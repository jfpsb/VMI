using System.Windows;

namespace VandaModaIntimaWpf.View.RecebimentoCartao
{
    /// <summary>
    /// Interaction logic for PesquisarRecebimento.xaml
    /// </summary>
    public partial class PesquisarRecebimento : APesquisarView2
    {
        public PesquisarRecebimento()
        {
            InitializeComponent();
        }

        private void TelaPesquisarRecebimento_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Recebimento de Cartão";
            window.Icon = GetIcon("/Resources/Credit_Card_Icon.ico");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closing += Pesquisar_Closing;
        }
    }
}
