using ACBrLib.PosPrinter;
using Gerencianet.NETCore.SDK;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.Pix;
using VandaModaIntimaWpf.Model.Pix;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Pix
{
    public class PagamentoPixVM : ObservableObject
    {
        private ISession _session;
        private DAOPix daoPix;
        private DAOCobranca daoCobranca;
        private ObservableCollection<Model.Pix.Pix> _listaPix;
        private double _valorQrCodePix;
        private bool _botaoGerarQrCodeEnabled = true;
        private IMessageBoxService messageBoxService;
        private IWindowService windowService;
        private double _totalPix;
        private ACBrPosPrinter _posPrinter;

        public ICommand GerarQRCodeComando { get; set; }
        public ICommand ConfigurarCredenciaisComando { get; set; }
        public ICommand ImprimirRelatorioComando { get; set; }
        public ICommand ListBoxPixLeftMouseClickComando { get; set; }
        public ICommand ImprimirRelatorioPixComando { get; set; }
        public ICommand ConsultarRecebimentoPixComando { get; set; }

        public PagamentoPixVM()
        {
            _session = SessionProvider.GetSession();
            daoPix = new DAOPix(_session);
            daoCobranca = new DAOCobranca(_session);
            messageBoxService = new MessageBoxService();
            windowService = new WindowService();

            var task = GetListaPix();
            task.Wait();

            GerarQRCodeComando = new RelayCommand(GerarQRCode, GerarQRCodeValidacao);
            ConfigurarCredenciaisComando = new RelayCommand(ConfigurarCredenciais);
            ImprimirRelatorioComando = new RelayCommand(ImprimirRelatorio);
            ListBoxPixLeftMouseClickComando = new RelayCommand(ListBoxPixLeftMouseClick);
            ImprimirRelatorioPixComando = new RelayCommand(ImprimirRelatorio);
            ConsultarRecebimentoPixComando = new RelayCommand(ConsultarRecebimentoPix);

            _posPrinter = new ACBrPosPrinter();
            ConfiguraPosPrinter.Configurar(_posPrinter);
        }

        private async void ConsultarRecebimentoPix(object obj)
        {
            AtualizarListaPixPelaGN();
            await GetListaPix();
        }

        private async void ImprimirRelatorio(object obj)
        {
            //Atualiza listas caso estejam desatualizadas
            //Ao mesmo tempo atualiza campos de total
            AtualizarCobrancasPelaGN();
            AtualizarListaPixPelaGN();
            await GetListaPix();

            int colunas = 48;
            string s = "</zera>" + "\n";
            s += "</ce>" + "\n";
            s += "</logo>" + "\n";
            s += "<e>RELATÓRIO PIX</e>" + "\n";
            s += "</ae>" + "\n";
            s += "<n>DATA</n>" + "\n";
            s += $"</fa>{DateTime.Now.ToString("d", CultureInfo.CurrentCulture)}" + "\n";
            s += "</pular_linhas>\n";
            s += "</ae>";
            s += "<e><n>Pagamentos por Pix</n></e>\n";

            if (ListaPix.Count > 0)
            {
                string horario = "Horário";
                string pagovia = "Pago Via";
                string valor = "Valor";

                s += string.Concat("<n>", horario, new string(' ', 13), pagovia, new string(' ', 15), valor, "</n>", "\n");

                foreach (var p in ListaPix)
                {
                    string data = p.HorarioLocalTime.ToString("T", CultureInfo.CurrentCulture);
                    string via = p.Txid == null ? "CHAVE" : "QR CODE";
                    string valorPix = p.Valor.ToString("C", CultureInfo.CurrentCulture);

                    int meio = colunas / 2;
                    int espacosAteSegundaPalavra = meio - (via.Length / 2) - data.Length;
                    int espacosAteTerceiraPalavra = colunas - meio - (via.Length / 2) - valorPix.Length - 1;

                    s += string.Concat("</fn>", data, new string(' ', espacosAteSegundaPalavra), via, new string(' ', espacosAteTerceiraPalavra), valorPix, "\n");
                }
            }
            else
            {
                s += "</fn>Não Houve Pagamentos No Dia\n";
            }

            s += "</pular_linhas>\n";

            s += $"</ad><e><n>TOTAL: {TotalPix.ToString("C", CultureInfo.CurrentCulture)}</n></e>\n";
            s += "</pular_linhas>\n";
            s += "</pular_linhas>\n";
            s += "</corte_total>\n";

            try
            {
                _posPrinter.Imprimir(s);
            }
            catch (Exception ex)
            {
                messageBoxService.Show("Erro ao imprimir relatório Pix. Cheque se a impressora está conectada corretamente e que está ligada.\n\n" + ex.Message, "Impressão De QR Code Pix", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Consulta A API da GerenciaNet para retornar os Pix do dia atual
        /// </summary>
        private async void AtualizarListaPixPelaGN()
        {
            var gnEndPoints = Config.GNEndpoints(_session);
            var dados = Config.GetDadosRecebedor(Config.LojaAplicacao(_session));

            if (gnEndPoints == null)
            {
                messageBoxService.Show($"Erro ao recuperar credenciais da GerenciaNet.\nAcesse {Log.LogCredenciais} para mais detalhes.", "Erro em Credenciais GerenciaNet", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dynamic endpoints = new Endpoints(gnEndPoints);

            var param = new
            {
                inicio = JsonConvert.SerializeObject(DateTime.Now.Date.ToUniversalTime()).Replace("\"", ""),
                fim = JsonConvert.SerializeObject(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToUniversalTime()).Replace("\"", "")
            };

            try
            {
                var response = endpoints.PixListReceived(param);
                ListaPixs listaPixs = JsonConvert.DeserializeObject<ListaPixs>(response);
                IList<Model.Pix.Pix> pixAtt = new List<Model.Pix.Pix>();

                foreach (var pixConsulta in listaPixs.Pixs.Where(w => w.Chave == null || w.Chave.ToLower().Equals((string)dados["chave_estatica"])))
                {
                    var pixLocal = await daoPix.ListarPorId(pixConsulta.EndToEndId);
                    if (pixLocal != null) continue; //Não insere pix que já existem no banco local
                    pixAtt.Add(pixConsulta); //Novo pix para inserir no banco
                }

                if (pixAtt.Count > 0)
                {
                    try
                    {
                        await daoPix.Inserir(pixAtt);
                    }
                    catch (Exception ex)
                    {
                        messageBoxService.Show(ex.Message, "Erro ao inserir/listar pix", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (GnException e)
            {
                Log.EscreveLogGn(e);
                messageBoxService.Show($"Erro ao listar pix da instituição GerenciaNet.\n\nAcesse {Log.LogGn} para mais detalhes.", "Erro ao consultar pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JsonReaderException jex)
            {
                Log.EscreveExceptionGenerica(jex);
                messageBoxService.Show($"Erro ao listar pix da instituição GerenciaNet. Cheque se está conectado a internet.\n\n{jex.Message}", "Erro ao Consultar Pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Log.EscreveExceptionGenerica(ex);
                messageBoxService.Show($"Erro ao listar pix da instituição GerenciaNet. Cheque se está conectado a internet.\n\n{ex.Message}\n\n{ex.InnerException?.Message}", "Erro ao Consultar Pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Consulta A API da GerenciaNet para retornar as cobranças do dia atual com seus respectivos status
        /// </summary>
        /// <param name="throwEx">Determina se eventuais exceções devem mostrar aviso ao usuário ou se serão tratadas silenciosamente</param>
        private async void AtualizarCobrancasPelaGN()
        {
            var gnEndPoints = Config.GNEndpoints(_session);

            if (gnEndPoints == null)
            {
                messageBoxService.Show($"Erro ao recuperar credenciais da GerenciaNet.\nAcesse {Log.LogCredenciais} para mais detalhes.", "Erro em Credenciais GerenciaNet", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dynamic endpoints = new Endpoints(gnEndPoints);

            var param = new
            {
                inicio = JsonConvert.SerializeObject(DateTime.Now.Date.ToUniversalTime()).Replace("\"", ""),
                fim = JsonConvert.SerializeObject(DateTime.Now.Date.AddDays(1).AddSeconds(-1).ToUniversalTime()).Replace("\"", "")
            };

            try
            {
                var response = endpoints.PixListCharges(param);
                ListaCobrancas listaCobranca = JsonConvert.DeserializeObject<ListaCobrancas>(response);
                IList<Cobranca> cobrancasAtt = new List<Cobranca>();

                foreach (var cobrancaConsulta in listaCobranca.Cobrancas)
                {
                    var cobrancaLocal = await daoCobranca.ListarPorId(cobrancaConsulta.Txid);

                    //Cobrança já existe no banco local
                    if (cobrancaLocal != null)
                    {
                        cobrancaLocal.Pix.Clear();
                        foreach (var p in cobrancaConsulta.Pix)
                        {
                            var pixLocal = await daoPix.ListarPorId(p.EndToEndId);

                            if (pixLocal == null)
                            {
                                p.Cobranca = cobrancaLocal;
                                cobrancaLocal.Pix.Add(p);
                            }
                            else
                            {
                                pixLocal.Cobranca = cobrancaLocal;
                                cobrancaLocal.Pix.Add(pixLocal);
                            }
                        }

                        cobrancaLocal.Revisao = cobrancaConsulta.Revisao;
                        cobrancaLocal.Status = cobrancaConsulta.Status;
                        cobrancaLocal.Chave = cobrancaConsulta.Chave;
                        cobrancaLocal.Location = cobrancaConsulta.Location;

                        if (cobrancaConsulta.Pix.Count > 0)
                            cobrancaLocal.PagoEm = cobrancaConsulta.Pix[0].Horario;

                        cobrancasAtt.Add(cobrancaLocal);
                    }
                    else
                    {
                        foreach (var p in cobrancaConsulta.Pix)
                        {
                            p.Cobranca = cobrancaConsulta;
                        }

                        if (cobrancaConsulta.Pix.Count > 0)
                            cobrancaConsulta.PagoEm = cobrancaConsulta.Pix[0].Horario;

                        cobrancasAtt.Add(cobrancaConsulta);
                    }
                }

                try
                {
                    await daoCobranca.InserirOuAtualizar(cobrancasAtt);
                }
                catch (Exception ex)
                {
                    messageBoxService.Show(ex.Message, "Erro ao inserir ou atualizar cobranças", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (GnException e)
            {
                Log.EscreveLogGn(e);
                messageBoxService.Show($"Erro ao listar cobranças da instituição GerenciaNet.\n\nAcesse {Log.LogGn} para mais detalhes.", "Erro ao consultar cobranças", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JsonReaderException jex)
            {
                messageBoxService.Show($"Erro ao listar cobranças da instituição GerenciaNet. Cheque se está conectado a internet.\n\n{jex.Message}", "Erro ao Consultar Cobranças Pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                messageBoxService.Show($"Erro ao listar cobranças da instituição GerenciaNet. Cheque se está conectado a internet.\n\n{ex.Message}\n\n{ex.InnerException?.Message}", "Erro ao Consultar Cobranças Pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBoxPixLeftMouseClick(object obj)
        {
            if (obj != null)
            {
                windowService.ShowDialog(new MaisDetalhesPixVM(obj as Model.Pix.Pix), null);
            }
        }

        private void ConfigurarCredenciais(object obj)
        {
            windowService.ShowDialog(new ConfiguraCredenciaisPixVM(Config.LojaAplicacao(_session)), null);
        }

        private async void GerarQRCode(object obj)
        {
            _botaoGerarQrCodeEnabled = false;

            try
            {
                if (ValorQrCodePix <= 0)
                    throw new FormatException("Valor de Pix não pode ser menor ou igual a zero.");

                var gnEndPoints = Config.GNEndpoints(_session);

                if (gnEndPoints == null)
                {
                    messageBoxService.Show($"Erro ao recuperar credenciais da GerenciaNet.\nAcesse {Log.LogCredenciais} para mais detalhes.", "Erro em Credenciais GerenciaNet", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dynamic endpoints = new Endpoints(gnEndPoints);
                var dados = Config.GetDadosRecebedor(Config.LojaAplicacao(_session));

                var body = new
                {
                    calendario = new
                    {
                        expiracao = 150 //2 e meio minutos
                    },
                    valor = new
                    {
                        original = ValorQrCodePix.ToString("F2", CultureInfo.InvariantCulture)
                    },
                    chave = (string)dados["chave"]
                };

                var cobrancaPix = endpoints.PixCreateImmediateCharge(null, body);
                Model.Pix.Cobranca cobranca = JsonConvert.DeserializeObject<Model.Pix.Cobranca>(cobrancaPix);

                try
                {
                    await daoCobranca.Inserir(cobranca);
                    await daoCobranca.RefreshEntidade(cobranca);
                    ApresentaQRCodePixVM dadosPixViewModel = new ApresentaQRCodePixVM(_session, cobranca.Id, messageBoxService);
                    windowService.ShowDialog(dadosPixViewModel, async (result, vm) =>
                    {
                        await GetListaPix();
                    });
                }
                catch (Exception ex)
                {
                    messageBoxService.Show(ex.Message, "Informe O Valor Do Pix", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (GnException e)
            {
                Log.EscreveLogGn(e);
                messageBoxService.Show($"Erro ao criar cobrança Pix na GerenciaNet.\nAcesse {Log.LogGn} para mais detalhes.", "Erro ao criar cobrança", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException fex)
            {
                messageBoxService.Show(fex.Message, "Criar Cobrança Pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                messageBoxService.Show($"Não Foi Possível Criar Cobrança Pix. Cheque Sua Conexão Com A Internet.\n\n{ex.GetType().Name}\n{ex.Message}\n{ex}", "Criar Cobrança Pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _botaoGerarQrCodeEnabled = true;
        }

        private bool GerarQRCodeValidacao(object arg)
        {
            return _botaoGerarQrCodeEnabled;
        }

        private async Task GetListaPix()
        {
            ListaPix = new ObservableCollection<Model.Pix.Pix>(await daoPix.ListarPixPorDiaLoja(DateTime.Now, Config.LojaAplicacao(_session)));
            TotalPix = ListaPix.Sum(s => s.Valor);
        }

        public void AoFecharTela()
        {
            SessionProvider.FechaSession(_session);
            if (_posPrinter != null)
                _posPrinter.Dispose();
        }

        public ObservableCollection<Model.Pix.Pix> ListaPix
        {
            get
            {
                return _listaPix;
            }

            set
            {
                _listaPix = value;
                OnPropertyChanged("ListaPix");
            }
        }

        public double ValorQrCodePix
        {
            get
            {
                return _valorQrCodePix;
            }

            set
            {
                _valorQrCodePix = value;
                OnPropertyChanged("ValorQrCodePix");
            }
        }

        public string ChaveEstatica
        {
            get => Config.ChaveEstatica();
        }

        public double TotalPix
        {
            get
            {
                return _totalPix;
            }

            set
            {
                _totalPix = value;
                OnPropertyChanged("TotalPix");
            }
        }
    }
}
