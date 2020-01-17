using NHibernate;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Arquivo;

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Classe abstrata para ViewModels de pesquisa
    /// </summary>
    public abstract class APesquisarViewModel : ObservableObject, IPesquisarViewModel
    {
        protected ISession _session;
        private string statusBarText;
        private string termoPesquisa;
        private bool _threadLocked;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        private string formId;
        protected ExcelStrategy excelStrategy;
        public ICommand AbrirCadastrarComando { get; set; }
        public ICommand AbrirApagarComando { get; set; }
        public ICommand AbrirEditarComando { get; set; }
        public ICommand AbrirAjudaComando { get; set; }
        public ICommand ChecarItensMarcadosComando { get; set; }
        public ICommand ApagarMarcadosComando { get; set; }
        public ICommand ExportarExcelComando { get; set; }
        public ICommand ImportarExcelComando { get; set; }
        public APesquisarViewModel(string formId)
        {
            AbrirCadastrarComando = new RelayCommand(AbrirCadastrar);
            AbrirApagarComando = new RelayCommand(AbrirApagarMsgBox);
            AbrirEditarComando = new RelayCommand(AbrirEditar);
            ChecarItensMarcadosComando = new RelayCommand(ChecarItensMarcados);
            ApagarMarcadosComando = new RelayCommand(ApagarMarcados);
            ExportarExcelComando = new RelayCommand(ExportarExcel);
            ImportarExcelComando = new RelayCommand(ImportarExcel);
            AbrirAjudaComando = new RelayCommand(AbrirAjuda);

            this.formId = formId;
            _session = SessionProvider.GetSession(formId);
        }

        public abstract void AbrirCadastrar(object parameter);
        public abstract void AbrirApagarMsgBox(object parameter);
        public abstract void AbrirEditar(object parameter);
        public abstract void ChecarItensMarcados(object parameter);
        public abstract void ApagarMarcados(object parameter);
        public virtual void ExportarExcel(object parameter)
        {
            SetStatusBarAguardandoExcel();
        }
        public abstract void ImportarExcel(object parameter);
        public abstract void GetItems(string termo);
        public abstract void SetStatusBarItemApagado();
        public abstract void AbrirAjuda(object parameter);
        public async Task ResetarStatusBar()
        {
            await Task.Delay(7000); //Espera 7 segundos para resetar StatusBar
            StatusBarText = "Aguardando Usuário";
        }
        public void SetStatusBarAguardandoExcel()
        {
            StatusBarText = "O Arquivo Excel Está Sendo Gerado. Aguarde a Tela Para Salvar.";
        }
        public async void SetStatusBarExportadoComSucesso()
        {
            StatusBarText = "Exportação em Excel Realizada Com Sucesso.";
            await Task.Delay(7000);
            StatusBarText = "Aguardando Usuário";
        }
        protected void PesquisarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TermoPesquisa":
                    GetItems(termoPesquisa);
                    break;
            }
        }
        public string TermoPesquisa
        {
            get { return termoPesquisa; }
            set
            {
                termoPesquisa = value;
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public bool IsThreadLocked
        {
            get { return _threadLocked; }
            set
            {
                _threadLocked = value;
                OnPropertyChanged("IsThreadLocked");
            }
        }
        public string StatusBarText
        {
            get { return statusBarText; }
            set
            {
                statusBarText = value;
                OnPropertyChanged("StatusBarText");
            }
        }
        public Visibility VisibilidadeBotaoApagarSelecionado
        {
            get { return visibilidadeBotaoApagarSelecionado; }
            set
            {
                visibilidadeBotaoApagarSelecionado = value;
                OnPropertyChanged("VisibilidadeBotaoApagarSelecionado");
            }
        }
        public void DisposeSession()
        {
            SessionProvider.FechaSession(this.formId);
        }

        bool IPesquisarViewModel.IsThreadLocked()
        {
            return _threadLocked;
        }
    }
}
