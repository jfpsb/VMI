using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Loja
{
    /// <summary>
    /// Interaction logic for CadastrarLoja.xaml
    /// </summary>
    public partial class SalvarLoja : Window
    {
        public SalvarLoja()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.Loja>)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCnpj.Focus();
        }

        private void TxtCnpj_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TxtCnpj.CaretIndex = TxtCnpj.Text.Length;
        }
    }
}
