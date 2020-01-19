using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Loja;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.View.Produto;

namespace VandaModaIntimaWpf.ViewModel
{
    class VandaModaIntimaViewModel : ObservableObject
    {
        public ICommand AbrirTelaProdutoComando { get; set; }
        public ICommand AbrirTelaFornecedorComando { get; set; }
        public ICommand AbrirTelaMarcaComando { get; set; }
        public ICommand AbrirTelaLojaComando { get; set; }
        public VandaModaIntimaViewModel()
        {
            AbrirTelaProdutoComando = new RelayCommand(AbrirTelaProduto);
            AbrirTelaFornecedorComando = new RelayCommand(AbrirTelaFornecedor);
            AbrirTelaMarcaComando = new RelayCommand(AbrirTelaMarca);
            AbrirTelaLojaComando = new RelayCommand(AbrirTelaLoja);
        }
        public void AbrirTelaProduto(object parameter)
        {
            PesquisarProduto pesquisarProduto = new PesquisarProduto();
            pesquisarProduto.Show();
        }
        public void AbrirTelaFornecedor(object parameter)
        {
            PesquisarFornecedor pesquisarFornecedor = new PesquisarFornecedor();
            pesquisarFornecedor.Show();
        }
        public void AbrirTelaMarca(object parameter)
        {
            PesquisarMarca pesquisarMarca = new PesquisarMarca();
            pesquisarMarca.Show();
        }
        public void AbrirTelaLoja(object parameter)
        {
            PesquisarLoja pesquisarLoja = new PesquisarLoja();
            pesquisarLoja.Show();
        }
        public void AbrirTelaRecebimento(object parameter)
        {

        }
    }
}
