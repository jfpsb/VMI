using System.Windows;

namespace VandaModaIntimaWpf.View.CompraDeFornecedor
{
    /// <summary>
    /// Interaction logic for PesquisarCompraDeFornecedor.xaml
    /// </summary>
    public partial class PesquisarCompraDeFornecedor : APesquisarView2
    {
        public PesquisarCompraDeFornecedor()
        {
            InitializeComponent();
        }

        private void TelaPesquisarCompraDeFornecedor_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Compras De Fornecedor";
            window.WindowState = WindowState.Maximized;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closing += Pesquisar_Closing;
        }
    }
}
