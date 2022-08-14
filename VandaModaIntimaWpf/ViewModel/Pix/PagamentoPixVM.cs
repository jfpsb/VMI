using Gerencianet.NETCore.SDK;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.Pix;
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

        public ICommand GerarQRCodeComando { get; set; }
        public ICommand ConfigurarCredenciaisComando { get; set; }

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
                    windowService.ShowDialog(dadosPixViewModel, null);
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
            var dt = new DateTime(2022, 8, 5);
            ListaPix = new ObservableCollection<Model.Pix.Pix>(await daoPix.ListarPixPorDiaLoja(dt, Config.LojaAplicacao(_session)));
        }

        public void FechaSession()
        {
            SessionProvider.FechaSession(_session);
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
    }
}
