using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Funcionario
{
    /// <summary>
    /// Interaction logic for CadastrarFuncionario.xaml
    /// </summary>
    public partial class SalvarFuncionario : ACadastrarView
    {
        public SalvarFuncionario()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCpf.Focus();
        }
    }
}
