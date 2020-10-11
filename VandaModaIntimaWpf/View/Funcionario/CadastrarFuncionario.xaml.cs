using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Funcionario
{
    /// <summary>
    /// Interaction logic for CadastrarFuncionario.xaml
    /// </summary>
    public partial class CadastrarFuncionario : Window
    {
        //TODO: implementar ViewModel, atualizar Model
        public CadastrarFuncionario()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = ((ACadastrarViewModel<Model.Funcionario>)DataContext).ResultadoSalvar();
            if (result != null)
                DialogResult = true;
            else
                DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCpf.Focus();
        }
    }
}
