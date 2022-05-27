using System.Windows;

namespace VandaModaIntimaWpf.View.Contagem
{
    /// <summary>
    /// Interaction logic for PesquisarContagem.xaml
    /// </summary>
    public partial class PesquisarContagem : APesquisarView2
    {
        public PesquisarContagem()
        {
            InitializeComponent();
        }

        private void TelaPesquisarContagem_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Contagem";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Icon = GetIcon("/Resources/Contagem_Icon.ico");
            window.Closing += Pesquisar_Closing;
        }
    }
}
