using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for CadastrarProduto.xaml
    /// </summary>
    public partial class CadastrarProduto : Window
    {
        public CadastrarProduto()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel)DataContext).ResultadoSalvar();
        }
    }
}
