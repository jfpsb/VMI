using NHibernate;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        protected ExcelStrategy excelStrategy;
        private string mensagemStatusBar;
        private string termoPesquisa;
        private bool _threadLocked;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        private string formId;
        private string imagemStatusBar;
        private DataGridCellInfo celulaSelecionada;

        public DataGridCellInfo CelulaSelecionada
        {
            get { return celulaSelecionada; }
            set
            {
                celulaSelecionada = value;
                OnPropertyChanged("CelulaSelecionada");
            }
        }

        protected static readonly string IMAGEMSUCESSO = "/Resources/Sucesso.png";
        protected static readonly string IMAGEMERRO = "/Resources/Erro.png";
        protected static readonly string IMAGEMAGUARDANDO = "/Resources/Aguardando.png";
        protected static readonly string IMAGEMDELETADO = "/Resources/Delete.png";
        public ICommand AbrirCadastrarComando { get; set; }
        public ICommand AbrirApagarComando { get; set; }
        public ICommand AbrirEditarComando { get; set; }
        public ICommand AbrirAjudaComando { get; set; }
        public ICommand ChecarItensMarcadosComando { get; set; }
        public ICommand ApagarMarcadosComando { get; set; }
        public ICommand ExportarExcelComando { get; set; }
        public ICommand ImportarExcelComando { get; set; }
        public ICommand CopiarValorCelulaComando { get; set; }
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
            CopiarValorCelulaComando = new RelayCommand(CopiarValorCelula);

            this.formId = formId;
            _session = SessionProvider.GetSession(formId);

            SetStatusBarAguardando();
        }

        public abstract void AbrirCadastrar(object parameter);
        public abstract void AbrirApagarMsgBox(object parameter);
        public abstract void AbrirEditar(object parameter);
        public abstract void ChecarItensMarcados(object parameter);
        public abstract void ApagarMarcados(object parameter);
        public abstract void AbrirAjuda(object parameter);
        public abstract void ImportarExcel(object parameter);
        public abstract void GetItems(string termo);
        public void CopiarValorCelula(object paramenter)
        {
            string valorCelula = (CelulaSelecionada.Column.GetCellContent(CelulaSelecionada.Item) as TextBlock).Text;
            Clipboard.SetText(valorCelula);
        }
        public virtual void ExportarExcel(object parameter)
        {
            SetStatusBarAguardandoExcel();
        }
        public void SetStatusBarItemDeletado(string mensagem)
        {
            MensagemStatusBar = mensagem;
            ImagemStatusBar = IMAGEMDELETADO;
        }
        public async Task ResetarStatusBar()
        {
            await Task.Delay(7000); //Espera 7 segundos para resetar StatusBar
            SetStatusBarAguardando();
        }
        public void SetStatusBarAguardandoExcel()
        {
            MensagemStatusBar = "O Arquivo Excel Está Sendo Gerado. Aguarde a Tela Para Salvar";
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }
        public void SetStatusBarAguardando()
        {
            MensagemStatusBar = "Aguardando Usuário";
            ImagemStatusBar = IMAGEMAGUARDANDO;
        }
        public async void SetStatusBarExportadoComSucesso()
        {
            MensagemStatusBar = "Exportação em Excel Realizada Com Sucesso";
            ImagemStatusBar = IMAGEMSUCESSO;
            await Task.Delay(7000);
            SetStatusBarAguardando();
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
        public string MensagemStatusBar
        {
            get { return mensagemStatusBar; }
            set
            {
                mensagemStatusBar = value;
                OnPropertyChanged("MensagemStatusBar");
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
        public string ImagemStatusBar
        {
            get { return imagemStatusBar; }
            set
            {
                imagemStatusBar = value;
                OnPropertyChanged("ImagemStatusBar");
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
