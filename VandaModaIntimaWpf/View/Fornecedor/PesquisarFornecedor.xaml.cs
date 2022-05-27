using System.Windows;

namespace VandaModaIntimaWpf.View.Fornecedor
{
    public partial class PesquisarFornecedor : APesquisarView2
    {
        public PesquisarFornecedor()
        {
            InitializeComponent();
        }

        private void TelaPesquisarFornecedor_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Fornecedor";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Icon = GetIcon("/Resources/Fornecedor_Icon.ico");
            window.Closing += Pesquisar_Closing;
        }
    }
}
