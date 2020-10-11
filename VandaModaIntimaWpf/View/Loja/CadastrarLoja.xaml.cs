using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Loja
{
    /// <summary>
    /// Interaction logic for CadastrarLoja.xaml
    /// </summary>
    public partial class CadastrarLoja : Window
    {
        public CadastrarLoja()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = ((ACadastrarViewModel<Model.Loja>)DataContext).ResultadoSalvar();
            if (result != null)
                DialogResult = true;
            else
                DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCnpj.Focus();
        }
    }
}
