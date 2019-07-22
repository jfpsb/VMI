using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Produto
{
    public partial class PesquisarProduto : Window
    {
        public PesquisarProduto()
        {
            InitializeComponent();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PesquisarProduto_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Fecha sessao
            ((IPesquisarViewModel)DataContext).DisposeSession();
        }
    }
}
