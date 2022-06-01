using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Marca
{
    /// <summary>
    /// Interaction logic for CadastrarMarca.xaml
    /// </summary>
    public partial class CadastrarMarca : ACadastrarView
    {
        public CadastrarMarca()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtMarca.Focus();
        }
    }
}
