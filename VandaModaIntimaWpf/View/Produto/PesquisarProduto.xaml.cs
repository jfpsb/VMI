namespace VandaModaIntimaWpf.View.Produto
{
    public partial class PesquisarProduto : APesquisarView
    {
        public PesquisarProduto()
        {
            InitializeComponent();
        }

        private void BtnCadastrarProduto_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CadastrarProduto cadastrarProduto = new CadastrarProduto();
            cadastrarProduto.ShowDialog();
        }
    }
}
