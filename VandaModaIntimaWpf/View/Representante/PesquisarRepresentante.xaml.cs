using System.Windows;

namespace VandaModaIntimaWpf.View.Representante
{
    /// <summary>
    /// Interaction logic for PesquisarRepresentante.xaml
    /// </summary>
    public partial class PesquisarRepresentante : APesquisarView2
    {
        public PesquisarRepresentante()
        {
            InitializeComponent();
        }

        private void TelaPesquisarRepresentante_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Representante";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closing += Pesquisar_Closing;
        }
    }
}
