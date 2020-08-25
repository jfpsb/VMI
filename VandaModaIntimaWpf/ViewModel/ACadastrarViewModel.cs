using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Resources;

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel : ObservableObject, ICadastrarViewModel
    {
        protected ISession _session;
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        protected CouchDbClient couchDbClient;
        private string mensagemStatusBar;
        private string imagemStatusBar;
        private string _botaoSalvarToolTip;

        protected bool _result = false; //Guarda se foi salvo com sucesso
        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        protected static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        protected CouchDbProdutoLog ultimoLog;

        public delegate void AposCriarDocumentoEventHandler(AposCriarDocumentoEventArgs e);
        public delegate void AposInserirBDEventHandler(AposInserirBDEventArgs e);

        public event AposCriarDocumentoEventHandler AposCriarDocumento;
        public event AposInserirBDEventHandler AposInserirBD;
        public ICommand SalvarComando { get; set; }
        public ACadastrarViewModel()
        {
            couchDbClient = new CouchDbClient();
            SalvarComando = new RelayCommand(Salvar, ValidacaoSalvar);
            SetStatusBarAguardando();
            AposCriarDocumento += InserirNoBancoDeDados;
            AposInserirBD += ResultadoInsercao;
            AposInserirBD += RedefinirTela;
        }

        public abstract void Salvar(object parameter);
        public abstract void ResetaPropriedades();
        public abstract bool ValidacaoSalvar(object parameter);
        public abstract void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);
        public abstract void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e);
        private async void ResultadoInsercao(AposInserirBDEventArgs e)
        {
            //Se foi inserido com sucesso
            if (e.CouchDbResponse.Ok)
            {
                //TODO: adicionar em lista para enviar por MQTT
            }
            else
            {
                if (e.IssoEUmUpdate)
                {
                    //Reverte criação de documento de update
                    CouchDbResponse couchDbResponse = await couchDbClient.UpdateDocument(e.CouchDbLog);
                    Console.WriteLine(string.Format("REVERTENDO UPDATE DE LOG {0}. Resultado: {1}", couchDbResponse.Id, couchDbResponse.Ok));
                }
                else
                {
                    //Reverte criação de documento
                    CouchDbResponse couchDbResponse = await couchDbClient.DeleteDocument(e.CouchDbResponse.Id, e.CouchDbResponse.Rev);
                    Console.WriteLine(string.Format("DELETANDO {0}: {1}", couchDbResponse.Id, couchDbResponse.Ok));
                }
            }
        }
        private async void RedefinirTela(AposInserirBDEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                if (!e.IssoEUmUpdate)
                    ResetaPropriedades();

                SetStatusBarSucesso(e.MensagemSucesso);
                await Task.Delay(5000); //Espera 5 segundos pra voltar com mensagem de aguardando usuário
                SetStatusBarAguardando();
            }
            else
            {
                SetStatusBarErro(e.MensagemErro);
            }
        }
        protected virtual void ChamaAposCriarDocumento(AposCriarDocumentoEventArgs e)
        {
            AposCriarDocumento?.Invoke(e);
        }

        protected virtual void ChamaAposInserirNoBD(AposInserirBDEventArgs e)
        {
            AposInserirBD?.Invoke(e);
        }
        protected void SetStatusBarSucesso(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMSUCESSO;
        }
        protected void SetStatusBarErro(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMERRO;
        }
        protected void SetStatusBarAguardando()
        {
            MensagemStatusBar = GetResource.GetString("aguardando_usuario");
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }
        protected void SetStatusBarAguardando(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }
        /// <summary>
        /// Método utilizado nas telas de ediçao para saber se houve edição.
        /// </summary>
        /// <returns>True se a entidade foi editada, senão, False</returns>
        public bool ResultadoSalvar()
        {
            return _result;
        }
        public string MensagemStatusBar
        {
            get { return mensagemStatusBar; }
            set
            {
                mensagemStatusBar = value;
                OnPropertyChanged("MensagemStatusBar");
            }
        }
        public string ImagemStatusBar
        {
            get { return imagemStatusBar; }
            set
            {
                imagemStatusBar = value;
                OnPropertyChanged("ImagemStatusBar");
            }
        }
        public Visibility VisibilidadeAvisoItemJaExiste
        {
            get { return visibilidadeAvisoItemJaExiste; }
            set
            {
                visibilidadeAvisoItemJaExiste = value;
                OnPropertyChanged("VisibilidadeAvisoItemJaExiste");
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        public string BotaoSalvarToolTip
        {
            get => _botaoSalvarToolTip;
            set
            {
                _botaoSalvarToolTip = value;
                OnPropertyChanged("BotaoSalvarToolTip");
            }
        }
    }
}
