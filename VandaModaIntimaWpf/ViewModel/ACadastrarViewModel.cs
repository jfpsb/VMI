using NHibernate;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel : ObservableObject, ICadastrarViewModel
    {
        protected ISession _session;
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        private string mensagemStatusBar;
        private string imagemStatusBar;

        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        protected static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        public ICommand CadastrarComando { get; set; }
        public ACadastrarViewModel()
        {
            CadastrarComando = new RelayCommand(Cadastrar, ValidaModel);
            SetStatusBarAguardando();
            _session = SessionProvider.GetSession();
        }
        public abstract void Cadastrar(object parameter);
        public abstract void ResetaPropriedades();
        public abstract bool ValidaModel(object parameter);
        public abstract void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);

        public async Task SetStatusBarSucesso()
        {
            MensagemStatusBar = "Cadastro Realizado Com Sucesso";
            ImagemStatusBar = IMAGEMSUCESSO;
            await Task.Delay(6000); //Espera 6 segundos pra voltar com mensagem de aguardando usuário
            SetStatusBarAguardando();
        }

        public void SetStatusBarAguardando()
        {
            MensagemStatusBar = "Aguardando Usuário";
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }

        public void SetStatusBarErro()
        {
            MensagemStatusBar = "Erro ao Cadastrar";
            ImagemStatusBar = IMAGEMERRO;
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
    }
}
