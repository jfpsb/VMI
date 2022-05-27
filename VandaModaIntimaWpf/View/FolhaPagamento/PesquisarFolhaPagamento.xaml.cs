using System.Windows;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    public partial class PesquisarFolhaPagamento : APesquisarView2
    {
        public PesquisarFolhaPagamento()
        {
            InitializeComponent();
        }

        private void TelaPesquisarFolhaPagamento_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Folha de Pagamento";
            window.Icon = GetIcon("/Resources/FolhaPagamento.png");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closing += Pesquisar_Closing;
        }
    }
}
