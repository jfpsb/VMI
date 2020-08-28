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

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel<E> : ObservableObject, ICadastrarViewModel where E : class, IModel, ICloneable
    {
        protected ISession _session;
        protected DAO daoEntidade;
        protected ICadastrarViewModelStrategy cadastrarViewModelStrategy;
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        protected CouchDbClient couchDbClient;
        protected E _entidade;
        protected bool _result = false; //Guarda se foi salvo com sucesso
        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        protected static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        protected CouchDbLog ultimoLog;

        private string mensagemStatusBar;
        private string imagemStatusBar;
        private string _botaoSalvarToolTip;

        public delegate void AntesDeCriarDocumentoEventHandler();
        public delegate void AposCriarDocumentoEventHandler(AposCriarDocumentoEventArgs e);
        public delegate void AposInserirBDEventHandler(AposInserirBDEventArgs e);

        public event AposCriarDocumentoEventHandler AposCriarDocumento;
        public event AposInserirBDEventHandler AposInserirBD;
        public event AntesDeCriarDocumentoEventHandler AntesDeCriarDocumento;
        public ICommand SalvarComando { get; set; }
        public ACadastrarViewModel(ISession session)
        {
            _session = session;
            couchDbClient = new CouchDbClient();
            SalvarComando = new RelayCommand(Salvar, ValidacaoSalvar);
            SetStatusBarAguardando();
            AntesDeCriarDocumento += ExecutarAntesCriarDocumento;
            AposCriarDocumento += InserirNoBancoDeDados;
            AposCriarDocumento += ConsultaUltimoLogNovamente;
            AposInserirBD += ResultadoInsercao;
            AposInserirBD += RedefinirTela;
            PropertyChanged += GetUltimoLog;
        }

        private void ConsultaUltimoLogNovamente(AposCriarDocumentoEventArgs e)
        {
            OnPropertyChanged("Entidade");
        }

        protected abstract void ExecutarAntesCriarDocumento();

        public virtual async void Salvar(object parameter)
        {
            ChamaAntesDeCriarDocumento();
            var e = await ExecutarSalvar();
            ChamaAposCriarDocumento(e);
        }
        protected async virtual Task<AposCriarDocumentoEventArgs> ExecutarSalvar()
        {
            string entidadeJson = JsonConvert.SerializeObject(Entidade);
            var couchDbResponse = await couchDbClient.CreateDocument(Entidade.CouchDbId(), entidadeJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = cadastrarViewModelStrategy.MensagemDocumentoCriadoComSucesso(),
                MensagemErro = cadastrarViewModelStrategy.MensagemDocumentoNaoCriado(),
                ObjetoSalvo = Entidade
            };

            return e;
        }
        protected async void GetUltimoLog(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Entidade":
                    ultimoLog = await couchDbClient.FindById(Entidade.CouchDbId());
                    break;
            }
        }
        public abstract void ResetaPropriedades();
        public abstract bool ValidacaoSalvar(object parameter);
        /// <summary>
        /// Executa toda vez que uma propriedade da entidade é modificada
        /// </summary>
        /// <param name="sender">Objeto Onde Originou Evento</param>
        /// <param name="e">Argumentos do Evento de PropertyChanged</param>
        public abstract void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e);
        /// <summary>
        /// Executa toda vez que uma propriedade da ViewModel é modificada
        /// </summary>
        /// <param name="sender">Objeto Onde Originou Evento</param>
        /// <param name="e">Argumentos do Evento de PropertyChanged</param>
        public abstract void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);
        public virtual async void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoEntidade.Inserir(Entidade);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    MensagemSucesso = cadastrarViewModelStrategy.MensagemEntidadeInseridaSucesso(),
                    MensagemErro = cadastrarViewModelStrategy.MensagemEntidadeErroAoInserir(),
                    ObjetoSalvo = Entidade,
                    CouchDbResponse = e.CouchDbResponse
                };

                ChamaAposInserirNoBD(e2);
            }
            else
            {
                SetStatusBarErro(e.MensagemErro);
            }
        }
        protected async Task AtualizarNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoEntidade.Merge(Entidade);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    IssoEUmUpdate = true,
                    MensagemSucesso = cadastrarViewModelStrategy.MensagemEntidadeAtualizadaSucesso(),
                    MensagemErro = cadastrarViewModelStrategy.MensagemEntidadeNaoAtualizada(),
                    ObjetoSalvo = Entidade,
                    CouchDbLog = e.CouchDbLog,
                    CouchDbResponse = e.CouchDbResponse
                };

                ChamaAposInserirNoBD(e2);
            }
            else
            {
                SetStatusBarErro(e.MensagemErro);
            }
        }
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
        protected virtual void ChamaAntesDeCriarDocumento()
        {
            AntesDeCriarDocumento?.Invoke();
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
