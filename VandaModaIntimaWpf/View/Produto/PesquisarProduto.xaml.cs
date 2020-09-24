namespace VandaModaIntimaWpf.View.Produto
{
    public partial class PesquisarProduto : APesquisarView
    {
        public PesquisarProduto()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TelaRelatorioProduto tela = new TelaRelatorioProduto();
            tela.Show();
        }
    }
}
