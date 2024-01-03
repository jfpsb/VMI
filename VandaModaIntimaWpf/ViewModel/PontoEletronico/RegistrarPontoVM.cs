using ACBrLib.PosPrinter;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.PontoEletronico
{
    public class RegistrarPontoVM : ACadastrarViewModel<Model.PontoEletronico>
    {
        private DateTime _horaAtual;
        private ObservableCollection<Model.PontoEletronico> _pontosEletronicos;
        private Model.PontoEletronico _pontoEletronico;
        private Timer horaAtualTimer;
        private DAOFuncionario daoFuncionario;
        private IList<Model.Funcionario> funcionarios;
        private IMessageBoxService messageBoxService;
        private IWindowService windowService;
        private ACBrPosPrinter posPrinter;

        private enum TipoPonto
        {
            Entrada,
            Saida,
            SaidaParaIntervalo,
            RetornoDeIntervalo
        }

        public ICommand RegistrarEntradaComando { get; set; }
        public ICommand RegistrarSaidaComando { get; set; }
        public ICommand RegistrarSaidaParaIntervaloComando { get; set; }
        public ICommand RegistrarRetornoDeIntervaloComando { get; set; }
        public ICommand TrocarSenhaComando { get; set; }
        public ICommand ExportarImprimirPontosComando { get; set; }

        public RegistrarPontoVM(ISession session, bool isUpdate = false) : base(session, isUpdate)
        {
            HoraAtual = DateTime.Now;
            horaAtualTimer = new Timer
            {
                Interval = 500.0
            };
            horaAtualTimer.Elapsed += HoraAtualTimer_Elapsed;
            horaAtualTimer.Start();

            daoEntidade = new DAOPontoEletronico(_session);
            daoFuncionario = new DAOFuncionario(_session);
            PontosEletronicos = new ObservableCollection<Model.PontoEletronico>();
            messageBoxService = new MessageBoxService();
            windowService = new WindowService();

            var task = GetFuncionarios();
            task.Wait();

            var task2 = PopulaListaDePontos();
            task2.Wait();

            RegistrarEntradaComando = new RelayCommand(RegistrarEntrada, RegistrarEntradaValidacao);
            RegistrarSaidaComando = new RelayCommand(RegistrarSaida, RegistrarSaidaValidacao);
            RegistrarSaidaParaIntervaloComando = new RelayCommand(RegistrarSaidaParaIntervalo, RegistrarSaidaParaIntervaloValidacao);
            RegistrarRetornoDeIntervaloComando = new RelayCommand(RegistrarRetornoDeIntervalo, RegistrarRetornoDeIntervaloValidacao);
            TrocarSenhaComando = new RelayCommand(TrocarSenha);
            ExportarImprimirPontosComando = new RelayCommand(ExportarImprimirPontos);

            posPrinter = new ACBrPosPrinter();
            ConfiguraPosPrinter.Configurar(posPrinter);
        }

        private void ExportarImprimirPontos(object obj)
        {
            Model.PontoEletronico ponto = obj as Model.PontoEletronico;
            TelaExportarImprimirPontoEletronicoVM vm = new TelaExportarImprimirPontoEletronicoVM(_session, posPrinter, ponto.Funcionario);
            windowService.ShowDialog(vm, null);
        }

        private void TrocarSenha(object obj)
        {
            windowService.ShowDialog(new TrocarSenhaFuncionarioVM(_session, PontoEletronico.Funcionario, messageBoxService), null);
        }

        private bool RegistrarRetornoDeIntervaloValidacao(object arg)
        {
            if (PontoEletronico == null)
                return false;

            if (PontoEletronico.Saida != null)
                return false;

            //Não há intervalos
            if (PontoEletronico.Intervalos.Count == 0)
                return false;

            //Não há intervalos com tempo de fim nulos
            var intervaloPonto = PontoEletronico.Intervalos.LastOrDefault();
            if (intervaloPonto.Fim != null)
                return false;

            return true;
        }
        private void RegistrarRetornoDeIntervalo(object obj)
        {
            windowService.ShowDialog(new InserirSenhaPontoVM(PontoEletronico.Funcionario), async (res, vm) =>
            {
                if (res == true)
                {
                    var intervaloPonto = PontoEletronico.Intervalos.LastOrDefault();
                    intervaloPonto.Fim = DateTime.Now;

                    try
                    {
                        await daoEntidade.InserirOuAtualizar(PontoEletronico);
                        ImprimirComprovanteRegistroPonto(PontoEletronico, TipoPonto.RetornoDeIntervalo);
                        messageBoxService.Show("Retorno de intervalo salvo com sucesso.");
                        await PopulaListaDePontos();
                    }
                    catch (Exception ex)
                    {
                        messageBoxService.Show($"Erro ao salvar retorno de intervalo.\n\n{ex.Message}");
                    }
                }
            });
        }
        private bool RegistrarSaidaParaIntervaloValidacao(object arg)
        {
            if (PontoEletronico == null)
                return false;

            if (PontoEletronico.Entrada == null)
                return false;

            if (PontoEletronico.Saida != null)
                return false;

            //Não pode abrir outro intervalo pois já há um em aberto
            var intervaloPonto = PontoEletronico.Intervalos.LastOrDefault();
            if (intervaloPonto != null && intervaloPonto.Fim == null)
                return false;

            return true;
        }
        private void RegistrarSaidaParaIntervalo(object obj)
        {
            windowService.ShowDialog(new InserirSenhaPontoVM(PontoEletronico.Funcionario), async (res, vm) =>
            {
                if (res == true)
                {
                    var intervaloPonto = PontoEletronico.Intervalos.LastOrDefault();
                    if (intervaloPonto == null || intervaloPonto.Fim != null)
                    {
                        intervaloPonto = new IntervaloPonto
                        {
                            PontoEletronico = PontoEletronico
                        };
                    }

                    intervaloPonto.Inicio = DateTime.Now;
                    PontoEletronico.Intervalos.Add(intervaloPonto);

                    try
                    {
                        await daoEntidade.InserirOuAtualizar(PontoEletronico);
                        ImprimirComprovanteRegistroPonto(PontoEletronico, TipoPonto.SaidaParaIntervalo);
                        messageBoxService.Show("Saída para intervalo salva com sucesso.");
                        await PopulaListaDePontos();
                    }
                    catch (Exception ex)
                    {
                        messageBoxService.Show($"Erro ao salvar saída para intervalo.\n\n{ex.Message}");
                    }
                }
            });
        }
        private bool RegistrarSaidaValidacao(object arg)
        {
            if (PontoEletronico == null)
                return false;

            if (PontoEletronico.Entrada == null)
                return false;

            if (PontoEletronico.Saida != null)
                return false;

            //Não pode sair se houver intervalo aberto
            var intervaloPonto = PontoEletronico.Intervalos.LastOrDefault();
            if (intervaloPonto != null && intervaloPonto.Fim == null)
                return false;

            return true;
        }
        private void RegistrarSaida(object obj)
        {
            windowService.ShowDialog(new InserirSenhaPontoVM(PontoEletronico.Funcionario), async (res, vm) =>
            {
                if (res == true)
                {
                    PontoEletronico.Saida = DateTime.Now;
                    try
                    {
                        await daoEntidade.InserirOuAtualizar(PontoEletronico);
                        ImprimirComprovanteRegistroPonto(PontoEletronico, TipoPonto.Saida);
                        await PopulaListaDePontos();
                    }
                    catch (Exception ex)
                    {
                        messageBoxService.Show($"Erro ao salvar saída.\n\n{ex.Message}");
                    }
                }
            });

        }
        private bool RegistrarEntradaValidacao(object arg)
        {
            if (PontoEletronico == null)
                return false;

            if (PontoEletronico.Entrada != null)
                return false;

            return true;
        }
        private void RegistrarEntrada(object obj)
        {
            windowService.ShowDialog(new InserirSenhaPontoVM(PontoEletronico.Funcionario), async (res, vm) =>
            {
                if (res == true)
                {
                    PontoEletronico.Entrada = DateTime.Now;
                    try
                    {
                        await daoEntidade.InserirOuAtualizar(PontoEletronico);
                        ImprimirComprovanteRegistroPonto(PontoEletronico, TipoPonto.Entrada);
                        await PopulaListaDePontos();
                    }
                    catch (Exception ex)
                    {
                        messageBoxService.Show($"Erro ao salvar entrada.\n\n{ex.Message}");
                    }
                }
            });
        }
        private async Task PopulaListaDePontos()
        {
            var dao = daoEntidade as DAOPontoEletronico;

            if (PontosEletronicos.Count == 0)
            {
                foreach (var f in funcionarios)
                {
                    var ponto = await dao.ListarPorDiaFuncionario(DateTime.Now, f);

                    if (ponto == null)
                    {
                        ponto = new Model.PontoEletronico
                        {
                            Funcionario = f,
                            Dia = DateTime.Now
                        };
                    }

                    PontosEletronicos.Add(ponto);
                }

                PontoEletronico = PontosEletronicos[0];
            }
            else
            {
                await daoEntidade.RefreshEntidade(PontosEletronicos);
                PontosEletronicos = new ObservableCollection<Model.PontoEletronico>(PontosEletronicos);
            }
        }
        private async Task GetFuncionarios()
        {
            var loja_trabalho = Config.LojaAplicacao(_session);
            funcionarios = await daoFuncionario.ListarPorLojaTrabalho(loja_trabalho);
        }
        private void ImprimirComprovanteRegistroPonto(Model.PontoEletronico ponto, TipoPonto tipoPonto)
        {
            string texto = "</zera>\n";
            texto += "</ce>\n";
            texto += "</logo>\n";
            texto += "<e>COMPROVANTE DE REGISTRO DE PONTO DO TRABALHADOR</e>\n";
            texto += "</ae>" + "\n";
            texto += "<i><n>EMPRESA</i>\n";
            texto += $"CNPJ</n>: {ponto.Funcionario.Loja.Cnpj}\n";
            texto += $"<n>Razão Social</n>: {ponto.Funcionario.Loja.RazaoSocial}\n";
            texto += $"<n>Endereço</n>: {ponto.Funcionario.Loja.Endereco}\n";
            texto += "<i><n>FUNCIONÁRIO</i>\n";
            texto += $"CPF</n>: {ponto.Funcionario.Cpf}\n";
            texto += $"<n>Nome</n>: {ponto.Funcionario.Nome}\n";
            texto += "</linha_dupla>\n";
            texto += "</ce><e>HORÁRIO DE REGISTRO</e>\n";

            var intervaloPonto = PontoEletronico.Intervalos.LastOrDefault();
            switch (tipoPonto)
            {
                case TipoPonto.Entrada:
                    texto += $"TIPO: ENTRADA\n";
                    texto += $"{ponto.Dia:dd/MM/yyyy} - {ponto.Entrada.Value:HH:mm}\n";
                    break;
                case TipoPonto.Saida:
                    texto += $"TIPO: SAÍDA\n";
                    texto += $"{ponto.Dia:dd/MM/yyyy} - {ponto.Saida.Value:HH:mm}\n";
                    break;
                case TipoPonto.SaidaParaIntervalo:
                    texto += $"TIPO: SAÍDA PARA INTERVALO\n";
                    texto += $"{ponto.Dia:dd/MM/yyyy} - {intervaloPonto.Inicio:HH:mm}\n";
                    break;
                case TipoPonto.RetornoDeIntervalo:
                    texto += $"TIPO: RETORNO DE INTERVALO\n";
                    texto += $"{ponto.Dia:dd/MM/yyyy} - {intervaloPonto.Fim.Value:HH:mm}\n";
                    break;
            }

            texto += "</linha_dupla>\n";
            texto += "</pular_linhas>\n";
            texto += "</corte_total>" + "\n";

            try
            {
                posPrinter.Imprimir(texto);
            }
            catch (Exception ex)
            {
                messageBoxService.Show("Erro ao imprimir comprovante. Cheque se a impressora está conectada corretamente e que está ligada.\n\n" + ex.Message, "Impressão De Comprovante Pix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void HoraAtualTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            HoraAtual = e.SignalTime;
        }
        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        public override void ResetaPropriedades(AposCRUDEventArgs e)
        {

        }
        public override bool ValidacaoSalvar(object parameter)
        {
            return false;
        }
        public DateTime HoraAtual
        {
            get
            {
                return _horaAtual;
            }

            set
            {
                _horaAtual = value;
                OnPropertyChanged("HoraAtual");
            }
        }
        public ObservableCollection<Model.PontoEletronico> PontosEletronicos
        {
            get
            {
                return _pontosEletronicos;
            }

            set
            {
                _pontosEletronicos = value;
                OnPropertyChanged("PontosEletronicos");
            }
        }
        public Model.PontoEletronico PontoEletronico
        {
            get
            {
                return _pontoEletronico;
            }

            set
            {
                _pontoEletronico = value;
                OnPropertyChanged("PontoEletronico");
            }
        }
    }
}
