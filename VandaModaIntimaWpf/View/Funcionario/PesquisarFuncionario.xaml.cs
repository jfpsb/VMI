using System.Windows;

namespace VandaModaIntimaWpf.View.Funcionario
{
    public partial class PesquisarFuncionario : APesquisarView2
    {
        public PesquisarFuncionario()
        {
            InitializeComponent();
        }

        private void TelaPesquisarFuncionario_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Funcionário";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Icon = GetIcon("/Resources/Funcionario_Icon.ico");
            window.Closing += Pesquisar_Closing;
        }
    }
}
