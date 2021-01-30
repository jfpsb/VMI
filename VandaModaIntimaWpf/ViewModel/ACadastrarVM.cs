using NHibernate;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.CouchDb;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel<E> : ObservableObject, ICadastrarVM where E : ObservableObject, IModel
    {
        protected ISession _session;
        protected DAO daoEntidade;
        protected ICadastrarVMStrategy viewModelStrategy;
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        protected CouchDbClient couchDbClient;
        protected E _entidade;
        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        protected static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        protected CouchDbLog UltimoLog;
        protected bool _result;

        private bool issoEUmUpdate;
        private string mensagemStatusBar;
        private string imagemStatusBar;
        private string _botaoSalvarToolTip;
        private string _btnSalvarToolTip;
        private IMessageBoxService MessageBoxService;

        public delegate void AntesDeCriarDocumentoEventHandler();
        public delegate void AntesDeInserirNoBancoDeDadosEventHandler();
        public delegate void AposCriarDocumentoEventHandler(AposSalvarDocumentoEventArgs e);
        public delegate void AposInserirNoBancoDeDadosEventHandler(AposSalvarEventArgs e);

        public event AposCriarDocumentoEventHandler AposCriarDocumento;
        public event AposInserirNoBancoDeDadosEventHandler AposInserirNoBancoDeDados;
        public event AntesDeCriarDocumentoEventHandler AntesDeCriarDocumento;
        public event AntesDeInserirNoBancoDeDadosEventHandler AntesDeInserirNoBancoDeDados;
        public ICommand SalvarComando { get; set; }
        /// <summary>
        /// Construtor abstrato para ViewModel de telas de cadastro de entidade
        /// </summary>
        /// <param name="session">Session do Hibernate que será usada na tela de cadastro</param>
        /// <param name="messageBoxService">Serviço de MessageBox que será usado na tela de cadastro</param>
        /// <param name="issoEUmUpdate">Marca se esta ViewModel está sendo usada em uma tela de cadastro ou tela de edição de entidade</param>
        public ACadastrarViewModel(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate)
        {
            _session = session;
            this.issoEUmUpdate = issoEUmUpdate;
            MessageBoxService = messageBoxService;
            couchDbClient = CouchDbClient.Instancia;
            SalvarComando = new RelayCommand(Salvar, ValidacaoSalvar);

            SetStatusBarAguardando();

            AposCriarDocumento += ResultadoSalvarDocumento;
            AposCriarDocumento += GetUltimoLogAposCriarDoc;
            AposCriarDocumento += SincronizarLocalComRemoto;

            AposInserirNoBancoDeDados += MensagemAposInserirNoBancoDeDados;
            AposInserirNoBancoDeDados += SalvarDocumento;
            AposInserirNoBancoDeDados += RedefinirTela;

            PropertyChanged += ACadastrarViewModel_PropertyChanged;

            issoEUmUpdate = false;
        }

        private void SincronizarLocalComRemoto(AposSalvarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
                Sincronizacao.SincronizaLocalComRemoto(_session);
        }

        private async void ACadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Entidade":
                    if (_entidade.CouchDbId() != null)
                        UltimoLog = await couchDbClient.FindById(Entidade.CouchDbId(), Entidade.GetType().Name.ToLower());
                    break;
            }
        }

        /// <summary>
        /// Escreve no console da aplicação se houve erro ou falha ao salvar documento do CouchDb
        /// </summary>
        /// <param name="e"></param>
        private void ResultadoSalvarDocumento(AposSalvarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                Console.WriteLine(viewModelStrategy.MensagemDocumentoSalvoComSucesso());
            }
            else
            {
                Console.WriteLine(viewModelStrategy.MensagemDocumentoNaoSalvo());
            }
        }
        /// <summary>
        /// Salva documento do CouchDb após operação de Salvar no Banco de Dados
        /// </summary>
        /// <param name="e"></param>
        private async void SalvarDocumento(AposSalvarEventArgs e)
        {
            if (e.Sucesso)
            {
                AntesDeCriarDocumento?.Invoke();

                CouchDbResponse couchDbResponse;

                if (UltimoLog == null)
                {
                    UltimoLog = new CouchDbLog
                    {
                        Id = Entidade.CouchDbId(),
                        TipoEntidade = Entidade.GetType().Name.ToLower(),
                        UltimaAlteracao = DateTime.Now,
                        Operacao = "insert"
                    };

                    couchDbResponse = await couchDbClient.CreateDocument(UltimoLog);
                }
                else
                {
                    UltimoLog.UltimaAlteracao = DateTime.Now;
                    UltimoLog.Sincronizado = false;
                    UltimoLog.Operacao = "update";
                    couchDbResponse = await couchDbClient.UpdateDocument(UltimoLog);
                }

                AposSalvarDocumentoEventArgs e2 = new AposSalvarDocumentoEventArgs()
                {
                    CouchDbResponse = couchDbResponse
                };

                AposCriarDocumento?.Invoke(e2);
            }
        }

        private void GetUltimoLogAposCriarDoc(AposSalvarDocumentoEventArgs e)
        {
            OnPropertyChanged("Entidade");
        }
        /// <summary>
        /// Método atrelado ao comando do botão Salvar
        /// </summary>
        /// <param name="parameter">Parâmetro da View</param>
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
        /// <summary>
        /// Executa a operação de salvar entidade
        /// </summary>
        /// <returns>Parâmetros do evento após inserção, em caso de sucesso ou falha</returns>
        protected async virtual Task<AposSalvarEventArgs> ExecutarSalvar()
        {
            _result = await daoEntidade.InserirOuAtualizar(Entidade);

            AposSalvarEventArgs e = new AposSalvarEventArgs()
            {
                IsUpdate = issoEUmUpdate,
                Sucesso = _result
            };

            return e;
        }

        public abstract void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e);

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
        /// <summary>
        /// Informa Ao Usuário O Resultado da Inserção
        /// </summary>
        /// <param name="e">Parâmetros do evento</param>
        private void MensagemAposInserirNoBancoDeDados(AposSalvarEventArgs e)
        {
            //Se foi inserido com sucesso
            if (e.Sucesso)
            {
                SetStatusBarSucesso(viewModelStrategy.MensagemEntidadeSalvaComSucesso());
            }
            else
            {
                SetStatusBarErro(viewModelStrategy.MensagemEntidadeErroAoSalvar());
            }
        }
        /// <summary>
        /// Redefine os campos do formulário para os valores padrões
        /// </summary>
        /// <param name="e"></param>
        protected async void RedefinirTela(AposSalvarEventArgs e)
        {
            if (e.Sucesso)
            {
                // Se a operação for um Update não há alteração no formulário
                if (!e.IsUpdate)
                    ResetaPropriedades();

                SetStatusBarSucesso(viewModelStrategy.MensagemEntidadeSalvaComSucesso());
                await Task.Delay(5000); //Espera 5 segundos pra voltar com mensagem de aguardando usuário
                SetStatusBarAguardando();
            }
            else
            {
                SetStatusBarErro(viewModelStrategy.MensagemEntidadeErroAoSalvar());
            }
        }
        /// <summary>
        /// Atribui a mensagem na StatusBar da tela de cadastrar/editar em caso de sucesso em uma operação
        /// </summary>
        /// <param name="mensagem">Mensagem a ser mostrada</param>
        protected void SetStatusBarSucesso(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMSUCESSO;
        }
        /// <summary>
        /// Atribui a mensagem na StatusBar da tela de cadastrar/editar em caso de erro em uma operação
        /// </summary>
        /// <param name="mensagem">Mensagem a ser mostrada</param>
        protected void SetStatusBarErro(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMERRO;
        }
        /// <summary>
        /// Atribui a mensagem na StatusBar da tela de cadastrar/editar mostrando que a aplicação está esperando que o usuário realize alguma ação
        /// </summary>
        protected void SetStatusBarAguardando()
        {
            MensagemStatusBar = GetResource.GetString("aguardando_usuario");
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }
        /// <summary>
        /// Atribui a mensagem na StatusBar da tela de cadastrar/editar mostrando que a aplicação está esperando que o usuário realize alguma ação
        /// </summary>
        /// <param name="mensagem">Mensagem a ser mostrada</param>
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

        public E Entidade
        {
            get
            {
                return _entidade;
            }

            set
            {
                _entidade = value;
                _entidade.PropertyChanged += Entidade_PropertyChanged;
                OnPropertyChanged("Entidade");
            }
        }

        public string BtnSalvarToolTip
        {
            get => _btnSalvarToolTip;
            set
            {
                _btnSalvarToolTip = value;
                OnPropertyChanged("BtnSalvarToolTip");
            }
        }
    }
}
