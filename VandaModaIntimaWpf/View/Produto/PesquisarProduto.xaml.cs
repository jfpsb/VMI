using System.Windows;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for PesquisarProduto.xaml
    /// </summary>
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
    }
}
