using NHibernate;
using System;
using System.ComponentModel;
using System.Drawing.Imaging;
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

        public delegate void AposCadastrarEventHandler(AposCadastrarEventArgs e);
        public event AposCadastrarEventHandler AposCadastrar;
        public ICommand SalvarComando { get; set; }
        public ACadastrarViewModel()
        {
            SalvarComando = new RelayCommand(Salvar, ValidacaoSalvar);
            SetStatusBarAguardando();
            AposCadastrar += RedefinirTela;
        }
        public abstract void Salvar(object parameter);
        public abstract void ResetaPropriedades();
        public abstract bool ValidacaoSalvar(object parameter);
        public abstract void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e);
        private async void RedefinirTela(AposCadastrarEventArgs e)
        {
            if (e.SalvoComSucesso)
            {
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
        protected virtual void ChamaAposCadastrar(AposCadastrarEventArgs e)
        {
            AposCadastrar?.Invoke(e);
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
        private void SetStatusBarAguardando()
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
