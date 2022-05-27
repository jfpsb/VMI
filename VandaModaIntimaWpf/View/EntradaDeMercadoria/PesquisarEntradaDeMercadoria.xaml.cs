using System.Windows;

namespace VandaModaIntimaWpf.View.EntradaDeMercadoria
{
    /// <summary>
    /// Interaction logic for PesquisarEntradaDeMercadoria.xaml
    /// </summary>
    public partial class PesquisarEntradaDeMercadoria : APesquisarView2
    {
        public PesquisarEntradaDeMercadoria()
        {
            InitializeComponent();
        }

        private void TelaPesquisarEntradaDeMercadoria_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Entrada De Mercadoria";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closing += Pesquisar_Closing;
        }
    }
}
