using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Classe abstrata para ViewModels de pesquisa
    /// </summary>
    public abstract class APesquisarViewModel : ObservableObject, IPesquisarViewModel
    {
        private string termoPesquisa;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        public ICommand AbrirCadastrarComando { get; set; }
        public ICommand AbrirApagarComando { get; set; }
        public ICommand AbrirEditarComando { get; set; }
        public ICommand ChecarItensMarcadosComando { get; set; }
        public ICommand ApagarMarcadosComando { get; set; }
        public ICommand ExportarExcelComando { get; set; }

        public APesquisarViewModel()
        {
            AbrirCadastrarComando = new RelayCommand(AbrirCadastrarNovo, IsCommandButtonEnabled);
            AbrirApagarComando = new RelayCommand(AbrirApagarMsgBox, IsCommandButtonEnabled);
            AbrirEditarComando = new RelayCommand(AbrirEditar, IsCommandButtonEnabled);
            ChecarItensMarcadosComando = new RelayCommand(ChecarItensMarcados, IsCommandButtonEnabled);
            ApagarMarcadosComando = new RelayCommand(ApagarMarcados, IsCommandButtonEnabled);
            ExportarExcelComando = new RelayCommand(ExportarExcel, IsCommandButtonEnabled);
        }

        public abstract void AbrirCadastrarNovo(object parameter);
        public abstract void AbrirApagarMsgBox(object parameter);
        public abstract void AbrirEditar(object parameter);
        public abstract void ChecarItensMarcados(object parameter);
        public abstract void ApagarMarcados(object parameter);
        public abstract void ExportarExcel(object parameter);
        public abstract void GetItems(string termo);
        public bool IsCommandButtonEnabled(object parameter)
        {
            return true;
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
            SessionProvider.FechaSession();
        }
    }
}
