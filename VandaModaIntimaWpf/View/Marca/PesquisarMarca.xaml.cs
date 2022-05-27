using System.Windows;

namespace VandaModaIntimaWpf.View.Marca
{
    /// <summary>
    /// Interaction logic for PesquisarMarca.xaml
    /// </summary>
    public partial class PesquisarMarca : APesquisarView2
    {
        public PesquisarMarca()
        {
            InitializeComponent();
        }

        private void TelaPesquisarMarca_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Marca";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ResizeMode = ResizeMode.NoResize;
            window.Closing += Pesquisar_Closing;
        }
    }
}
