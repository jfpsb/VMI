using System.Windows;

namespace VandaModaIntimaWpf.View.Loja
{
    public partial class PesquisarLoja : APesquisarView2
    {
        public PesquisarLoja()
        {
            InitializeComponent();
        }

        private void TelaPesquisarLoja_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Loja";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Icon = GetIcon("/Resources/Loja_Icon.ico");
            window.Closing += Pesquisar_Closing;
        }
    }
}
