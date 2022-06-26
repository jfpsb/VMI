using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.View.CompraDeFornecedor;
using VandaModaIntimaWpf.View.Contagem;
using VandaModaIntimaWpf.View.Despesa;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.View.Ferias;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Funcionario;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.View.RecebimentoCartao;

namespace VandaModaIntimaWpf.View
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
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionProvider.FechaSessionFactory();
        }

        private void BtnProduto_Click(object sender, RoutedEventArgs e)
        {
            PesquisarProduto pesquisarProduto = new PesquisarProduto();
            pesquisarProduto.Show();
        }

        private void BtnFornecedor_Click(object sender, RoutedEventArgs e)
        {
            PesquisarFornecedor pesquisarFornecedor = new PesquisarFornecedor();
            pesquisarFornecedor.Show();
        }

        private void BtnFuncionario_Click(object sender, RoutedEventArgs e)
        {
            PesquisarFuncionario pesquisarFuncionario = new PesquisarFuncionario();
            pesquisarFuncionario.Show();
        }

        private void BtnLoja_Click(object sender, RoutedEventArgs e)
        {
            PesquisarLoja pesquisarLoja = new PesquisarLoja();
            pesquisarLoja.Show();
        }

        private void BtnMarca_Click(object sender, RoutedEventArgs e)
        {
            PesquisarMarca pesquisarMarca = new PesquisarMarca();
            pesquisarMarca.Show();
        }

        private void BtnCompraDeFornecedor_Click(object sender, RoutedEventArgs e)
        {
            PesquisarCompraDeFornecedor pesquisarCompraDeFornecedor = new PesquisarCompraDeFornecedor();
            pesquisarCompraDeFornecedor.Show();
        }

        private void BtnRecebimentoCartao_Click(object sender, RoutedEventArgs e)
        {
            PesquisarRecebimento pesquisarRecebimento = new PesquisarRecebimento();
            pesquisarRecebimento.Show();
        }

        private void BtnContagem_Click(object sender, RoutedEventArgs e)
        {
            PesquisarContagem pesquisarContagem = new PesquisarContagem();
            pesquisarContagem.Show();
        }

        private void BtnDespesa_Click(object sender, RoutedEventArgs e)
        {
            PesquisarDespesa pesquisarDespesa = new PesquisarDespesa();
            pesquisarDespesa.Show();
        }

        private void BtnFolhaPagamento_Click(object sender, RoutedEventArgs e)
        {
            PesquisarFolhaPagamento pesquisarFolhaPagamento = new PesquisarFolhaPagamento();
            pesquisarFolhaPagamento.Show();
        }

        private void BtnEntradaDeMercadoria_Click(object sender, RoutedEventArgs e)
        {
            PesquisarEntradaDeMercadoria pesquisarEntradaDeMercadoria = new PesquisarEntradaDeMercadoria();
            pesquisarEntradaDeMercadoria.Show();
        }

        private void BtnFerias_Click(object sender, RoutedEventArgs e)
        {
            VisualizadorDeFerias visualizadorDeFerias = new VisualizadorDeFerias();
            visualizadorDeFerias.Show();
        }
    }
}
