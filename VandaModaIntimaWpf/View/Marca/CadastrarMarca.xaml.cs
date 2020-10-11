using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Marca
{
    /// <summary>
    /// Interaction logic for CadastrarMarca.xaml
    /// </summary>
    public partial class CadastrarMarca : Window
    {
        public CadastrarMarca()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = ((ACadastrarViewModel<Model.Marca>)DataContext).ResultadoSalvar();
            if (result != null)
                DialogResult = true;
            else
                DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtMarca.Focus();
        }
    }
}
