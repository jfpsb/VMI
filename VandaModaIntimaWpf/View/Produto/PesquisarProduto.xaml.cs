using System.Windows;
using VandaModaIntimaWpf.ViewModel.Produto;

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
            ((PesquisarProdutoViewModel)DataContext).DisposeSession();
        }
    }
}
