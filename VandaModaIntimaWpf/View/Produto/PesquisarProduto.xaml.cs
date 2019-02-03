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
            ((PesquisarProdutoViewModel)DataContext).DisposeServico();
        }

        private void CadastrarNovo_Click(object sender, RoutedEventArgs e)
        {
            CadastrarProduto cadastrarProduto = new CadastrarProduto();
            cadastrarProduto.ShowDialog();

            //Atribui o próprio texto do campo nele mesmo para somente executar
            //o evento no ViewModel para atualizar a consulta
            TxtPesquisa.Text = TxtPesquisa.Text;
        }
    }
}
