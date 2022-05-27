using System.Windows;

namespace VandaModaIntimaWpf.View.Produto
{
    public partial class PesquisarProduto : APesquisarView2
    {
        public PesquisarProduto()
        {
            InitializeComponent();
        }

        private void TelaPesquisarProduto_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            Application.Current.MainWindow.Width = 1250;
            Application.Current.MainWindow.Height = 700;
            window.Title = "Pesquisar Produto";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Icon = GetIcon("/Resources/Produto_Icon.ico");
            window.Closing += Pesquisar_Closing;
        }
    }
}
