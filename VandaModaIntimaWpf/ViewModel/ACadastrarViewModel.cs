using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    public abstract class ACadastrarViewModel : ObservableObject, ICadastrarViewModel
    {
        protected Visibility visibilidadeAvisoItemJaExiste = Visibility.Collapsed;
        protected bool isEnabled = true;
        private string mensagemStatusBar = "Aguardando Usuário";
        private string imagemStatusBar = IMAGEMAGUARDANDO;

        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        private static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        public ICommand CadastrarComando { get; set; }
        public ACadastrarViewModel()
        {
            CadastrarComando = new RelayCommand(Cadastrar, ValidaModel);
        }
        public abstract void Cadastrar(object parameter);
        public abstract void ResetaPropriedades();
        public abstract bool ValidaModel(object parameter);
        public abstract void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);
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
