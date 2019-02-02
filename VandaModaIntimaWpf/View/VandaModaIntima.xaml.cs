using System.Windows;
using VandaModaIntimaWpf.View.Produto;

namespace VandaModaIntimaWpf
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Produto_Click(object sender, RoutedEventArgs e)
        {
            PesquisarProduto pesquisarProduto = new PesquisarProduto();
            pesquisarProduto.Show();
        }
    }
}
