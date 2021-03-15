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
        protected CouchDbLog ultimoLog;
        protected bool _result;

        private bool issoEUmUpdate;
        private string mensagemStatusBar;
        private string imagemStatusBar;
        private string _botaoSalvarToolTip;
        private string _btnSalvarToolTip;
        protected IMessageBoxService MessageBoxService;

        public delegate void AntesDeCriarDocumentoEventHandler();
        public delegate void AntesDeInserirNoBancoDeDadosEventHandler();
        public delegate void AposCriarDocumentoEventHandler(AposCriarDocumentoEventArgs e);
        public delegate void AposInserirNoBancoDeDadosEventHandler(AposInserirBDEventArgs e);

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

            AposInserirNoBancoDeDados += ResultadoInsercao;
            //AposInserirNoBancoDeDados += SalvarDocumento;
            AposInserirNoBancoDeDados += RedefinirTela;

            //PropertyChanged += GetUltimoLogDeEntidade;

            issoEUmUpdate = false;
        }
        /// <summary>
        /// Escreve no console da aplicação se houve erro ou falha ao salvar documento do CouchDb
        /// </summary>
        /// <param name="e"></param>
        private void ResultadoSalvarDocumento(AposCriarDocumentoEventArgs e)
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
        /// <summary>
        /// Salva documento do CouchDb após operação de Salvar no Banco de Dados
        /// </summary>
        /// <param name="e"></param>
        private async void SalvarDocumento(AposInserirBDEventArgs e)
        {
            AntesDeCriarDocumento?.Invoke();
            CouchDbResponse couchDbResponse;

            E entidadeInserida = (E)await daoEntidade.ListarPorId(e.IdentificadorEntidade);

            if (ultimoLog == null)
            {
                string entidadeJson = JsonConvert.SerializeObject(entidadeInserida);
                couchDbResponse = await couchDbClient.CreateDocument(entidadeInserida.CouchDbId(), entidadeJson);
            }
            else
            {
                e.CouchDbLog = (CouchDbLog)ultimoLog.Clone();
                ultimoLog.AtribuiCampos(entidadeInserida);
                couchDbResponse = await couchDbClient.UpdateDocument(ultimoLog);
                e.CouchDbLog.Rev = couchDbResponse.Rev;
            }

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
        /// <summary>
        /// Método atrelado ao comando do botão Salvar
        /// </summary>
        /// <param name="parameter">Parâmetro da View</param>
        public virtual async void Salvar(object parameter)
        {
            try
            {
                AntesDeInserirNoBancoDeDados?.Invoke();
                var e = await ExecutarSalvar(parameter);
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
        protected async virtual Task<AposInserirBDEventArgs> ExecutarSalvar(object parametro)
        {
            _result = await daoEntidade.InserirOuAtualizar(Entidade);

            AposInserirBDEventArgs e = new AposInserirBDEventArgs()
            {
                IssoEUmUpdate = issoEUmUpdate,
                IdentificadorEntidade = Entidade.GetIdentifier(),
                MensagemSucesso = viewModelStrategy.MensagemEntidadeSalvaComSucesso(),
                MensagemErro = viewModelStrategy.MensagemEntidadeErroAoSalvar(),
                Sucesso = _result,
                Parametro = parametro
            };

            return e;
        }
        /// <summary>
        /// Consulta e atribui o último log do CouchDb salvo dessa entidade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Parâmetros de evento</param>
        protected async void GetUltimoLogDeEntidade(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Entidade":
                    if (_entidade.CouchDbId() != null)
                        ultimoLog = await couchDbClient.FindById(_entidade.CouchDbId());
                    break;
            }
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
        /// Checa o resultado da inserção da entidade e retorna ao usuário
        /// </summary>
        /// <param name="e">Parâmetros do evento</param>
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
        /// <summary>
        /// Redefine os campos do formulário para os valores padrões
        /// </summary>
        /// <param name="e"></param>
        private async void RedefinirTela(AposInserirBDEventArgs e)
        {
            if (e.IdentificadorEntidade != null)
            {
                // Se a operação for um Update não há alteração no formulário
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
