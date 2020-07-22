using NHibernate;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Resources;

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel : ObservableObject, ICadastrarViewModel
    {
        protected ISession _session;
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        private string mensagemStatusBar;
        private string imagemStatusBar;
        private string _botaoSalvarToolTip;

        protected bool _result = false; //Guarda se foi salvo com sucesso
        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        protected static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        public ICommand SalvarComando { get; set; }
        public ACadastrarViewModel()
        {
            SalvarComando = new RelayCommand(Salvar, ValidacaoSalvar);
            SetStatusBarAguardando(StringResource.GetString("aguardando_usuario"));
        }
        public abstract void Salvar(object parameter);
        public abstract void ResetaPropriedades();
        public abstract bool ValidacaoSalvar(object parameter);
        public abstract void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);

        public virtual async Task SetStatusBarSucesso(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMSUCESSO;
            await Task.Delay(5000); //Espera 5 segundos pra voltar com mensagem de aguardando usuário
            SetStatusBarAguardando(StringResource.GetString("aguardando_usuario"));
        }
        public void SetStatusBarAguardando(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }
        public virtual void SetStatusBarErro(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMERRO;
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
