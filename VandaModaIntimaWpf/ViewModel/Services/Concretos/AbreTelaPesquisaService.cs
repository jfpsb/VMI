using VandaModaIntimaWpf.View.CompraDeFornecedor;
using VandaModaIntimaWpf.View.Contagem;
using VandaModaIntimaWpf.View.Despesa;
using VandaModaIntimaWpf.View.EntradaDeMercadoria;
using VandaModaIntimaWpf.View.FolhaPagamento;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Funcionario;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.View.Produto;
using VandaModaIntimaWpf.View.RecebimentoCartao;
using VandaModaIntimaWpf.ViewModel.CompraDeFornecedor;
using VandaModaIntimaWpf.ViewModel.Contagem;
using VandaModaIntimaWpf.ViewModel.Despesa;
using VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Funcionario;
using VandaModaIntimaWpf.ViewModel.Loja;
using VandaModaIntimaWpf.ViewModel.Marca;
using VandaModaIntimaWpf.ViewModel.Produto;
using VandaModaIntimaWpf.ViewModel.RecebimentoCartao;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Services.Concretos
{
    public class AbreTelaPesquisaService : IAbreTelaPesquisaService
    {
        public void AbrirTelaContagem()
        {
            PesquisarContagemVM viewModel = new PesquisarContagemVM(new MessageBoxService(), new AbrePelaTelaPesqContagemService());
            PesquisarContagem pesquisarContagem = new PesquisarContagem() { DataContext = viewModel };
            pesquisarContagem.Show();
        }

        public void AbrirTelaDespesas()
        {
            PesquisarDespesaVM viewModel = new PesquisarDespesaVM(new MessageBoxService(), new AbrePelaTelaPesquisaDespesa());
            PesquisarDespesa view = new PesquisarDespesa { DataContext = viewModel };
            view.Show();
        }

        public void AbrirTelaCompraDeFornecedor()
        {
            PesquisarCompraDeFornecedorVM viewModel = new PesquisarCompraDeFornecedorVM(new MessageBoxService(), new AbrePelaTelaPesquisaCompraDeFornecedor());
            PesquisarCompraDeFornecedor view = new PesquisarCompraDeFornecedor() { DataContext = viewModel };
            view.Show();
        }

        public void AbrirTelaFolhaPagamento()
        {
            PesquisarFolhaVM viewModel = new PesquisarFolhaVM(new MessageBoxService(), new AbrePelaTelaPesqFolhaService());
            PesquisarFolhaPagamento pesquisarFolhaPagamento = new PesquisarFolhaPagamento() { DataContext = viewModel };
            pesquisarFolhaPagamento.Show();
        }

        public void AbrirTelaFornecedor()
        {
            PesquisarFornecedorVM viewModel = new PesquisarFornecedorVM(new MessageBoxService(), new AbrePelaTelaPesqFornecedorService());
            PesquisarFornecedor pesquisarFornecedor = new PesquisarFornecedor() { DataContext = viewModel };
            pesquisarFornecedor.Show();
        }

        public void AbrirTelaFuncionario()
        {
            PesquisarFuncionarioVM viewModel = new PesquisarFuncionarioVM(new MessageBoxService(), new AbrePelaTelaPesqFuncService());
            PesquisarFuncionario pesquisarFuncionario = new PesquisarFuncionario() { DataContext = viewModel };
            pesquisarFuncionario.Show();
        }

        public void AbrirTelaLoja()
        {
            PesquisarLojaVM viewModel = new PesquisarLojaVM(new MessageBoxService(), new AbrePelaTelaPesqLojaService());
            PesquisarLoja pesquisarLoja = new PesquisarLoja() { DataContext = viewModel };
            pesquisarLoja.Show();
        }

        public void AbrirTelaMarca()
        {
            PesquisarMarcaVM viewModel = new PesquisarMarcaVM(new MessageBoxService(), new AbrePelaTelaPesqMarcaService());
            PesquisarMarca pesquisarMarca = new PesquisarMarca() { DataContext = viewModel };
            pesquisarMarca.Show();
        }

        public void AbrirTelaProduto()
        {
            PesquisarProdutoVM viewModel = new PesquisarProdutoVM(new MessageBoxService(), new AbrePelaTelaPesqProdutoService());
            PesquisarProduto pesquisarProduto = new PesquisarProduto() { DataContext = viewModel };
            pesquisarProduto.Show();
        }

        public void AbrirTelaRecebimento()
        {
            PesquisarRecebimentoVM viewModel = new PesquisarRecebimentoVM(new MessageBoxService(), new AbrePelaTelaPesqRecebService());
            PesquisarRecebimento pesquisarRecebimento = new PesquisarRecebimento() { DataContext = viewModel };
            pesquisarRecebimento.Show();
        }

        public void AbrirTelaEntradaMercadoria()
        {
            PesquisarEntradaVM viewModel = new PesquisarEntradaVM(new MessageBoxService(), new AbrePelaTelaPesquisaEntradaMercadoriaService());
            PesquisarEntradaDeMercadoria view = new PesquisarEntradaDeMercadoria() { DataContext = viewModel };
            view.Show();
        }
    }
}
