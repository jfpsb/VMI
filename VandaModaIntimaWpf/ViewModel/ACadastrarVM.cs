using Newtonsoft.Json;
using NHibernate;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel<E> : ObservableObject, ICadastrarVM where E : class, IModel
    {
        protected ISession _session;
        protected DAO daoEntidade;
        protected ICadastrarVMStrategy viewModelStrategy;
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        protected CouchDbClient couchDbClient;
        protected E _entidade;
        protected object _identifier = null; //Guarda item salvo
        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        protected static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        protected CouchDbLog ultimoLog;

        private string mensagemStatusBar;
        private string imagemStatusBar;
        private string _botaoSalvarToolTip;
        private IMessageBoxService MessageBoxService;

        public delegate void AntesDeCriarDocumentoEventHandler();
        public delegate void AntesDeInserirNoBancoDeDadosEventHandler();
        public delegate void AposCriarDocumentoEventHandler(AposCriarDocumentoEventArgs e);
        public delegate void AposInserirNoBancoDeDadosEventHandler(AposInserirBDEventArgs e);

        public event AposCriarDocumentoEventHandler AposCriarDocumento;
        public event AposInserirNoBancoDeDadosEventHandler AposInserirNoBancoDeDados;
        public event AntesDeCriarDocumentoEventHandler AntesDeCriarDocumento;
        public event AntesDeInserirNoBancoDeDadosEventHandler AntesDeInserirNoBancoDeDados;
        public ICommand SalvarComando { get; set; }
        public ACadastrarViewModel(ISession session, IMessageBoxService messageBoxService)
        {
            _session = session;
            MessageBoxService = messageBoxService;
            couchDbClient = CouchDbClient.Instancia;
            SalvarComando = new RelayCommand(Salvar, ValidacaoSalvar);

            SetStatusBarAguardando();

            AposCriarDocumento += ResultadoCriacaoDocumento;
            AposCriarDocumento += GetUltimoLogAposCriarDoc;

            AposInserirNoBancoDeDados += ResultadoInsercao;
            AposInserirNoBancoDeDados += CriarDocumento;
            AposInserirNoBancoDeDados += RedefinirTela;

            PropertyChanged += GetUltimoLogDeEntidade;
        }

        private void ResultadoCriacaoDocumento(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                Console.WriteLine(e.MensagemSucesso);
            }
            else
            {
                Console.WriteLine(e.MensagemErro);
            }
        }

        private async void CriarDocumento(AposInserirBDEventArgs e)
        {
            AntesDeCriarDocumento?.Invoke();

            E entidadeInserida = (E)await daoEntidade.ListarPorId(e.IdentificadorEntidade);
            string entidadeJson = JsonConvert.SerializeObject(entidadeInserida);
            var couchDbResponse = await couchDbClient.CreateDocument(entidadeInserida.CouchDbId(), entidadeJson);

            AposCriarDocumentoEventArgs e2 = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = viewModelStrategy.MensagemDocumentoSalvoComSucesso(),
                MensagemErro = viewModelStrategy.MensagemDocumentoNaoSalvo(),
                IdentificadorEntidade = e.IdentificadorEntidade
            };

            AposCriarDocumento?.Invoke(e2);
        }

        private void GetUltimoLogAposCriarDoc(AposCriarDocumentoEventArgs e)
        {
            OnPropertyChanged("Entidade");
        }

        public virtual async void Salvar(object parameter)
        {
            try
            {
                AntesDeInserirNoBancoDeDados?.Invoke();
                var e = await ExecutarSalvar();
                AposInserirNoBancoDeDados?.Invoke(e);
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message);
            }
        }
        protected async virtual Task<AposInserirBDEventArgs> ExecutarSalvar()
        {
            _identifier = await daoEntidade.InserirOuAtualizar(Entidade);

            AposInserirBDEventArgs e = new AposInserirBDEventArgs()
            {
                IdentificadorEntidade = _identifier,
                MensagemSucesso = viewModelStrategy.MensagemEntidadeSalvaComSucesso(),
                MensagemErro = viewModelStrategy.MensagemEntidadeErroAoSalvar()
            };

            return e;
        }
        protected async void GetUltimoLogDeEntidade(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Entidade":
                    ultimoLog = await couchDbClient.FindById(Entidade.CouchDbId());
                    break;
            }
        }

        /// <summary>
        /// Retorna as propriedades da entidade a seus valores iniciais
        /// </summary>
        public abstract void ResetaPropriedades();

        /// <summary>
        /// Realiza os testes para determinar se todos os requisitos necessários para permitir o cadastro foram atingidos
        /// </summary>
        /// <param name="parameter">Parâmetro do comando</param>
        /// <returns></returns>
        public abstract bool ValidacaoSalvar(object parameter);

        private async void ResultadoInsercao(AposInserirBDEventArgs e)
        {
            //Se foi inserido com sucesso
            if (e.IdentificadorEntidade != null)
            {
                Entidade = (E)await daoEntidade.ListarPorId(e.IdentificadorEntidade);
                SetStatusBarSucesso(e.MensagemSucesso);
            }
            else
            {
                SetStatusBarErro(e.MensagemErro);
            }
        }
        private async void RedefinirTela(AposInserirBDEventArgs e)
        {
            if (e.IdentificadorEntidade != null)
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
        protected virtual void ChamaAposInserirNoBD(AposInserirBDEventArgs e)
        {
            AposInserirNoBancoDeDados?.Invoke(e);
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
        /// <returns>Objeto se a entidade foi editada, senão, null</returns>
        public object ResultadoSalvar()
        {
            return _identifier;
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

        public E Entidade
        {
            get
            {
                return _entidade;
            }

            set
            {
                _entidade = value;
                OnPropertyChanged("Entidade");
            }
        }
    }
}
