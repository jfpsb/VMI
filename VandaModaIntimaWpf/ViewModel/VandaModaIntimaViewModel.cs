using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Marca;
using VandaModaIntimaWpf.ViewModel.Produto;

namespace VandaModaIntimaWpf.ViewModel
{
    class VandaModaIntimaViewModel : ObservableObject
    {
        public ICommand AbrirTelaProdutoComando { get; set; }
        public ICommand AbrirTelaFornecedorComando { get; set; }
        public ICommand AbrirTelaMarcaComando { get; set; }
        public VandaModaIntimaViewModel()
        {
            AbrirTelaProdutoComando = new RelayCommand(AbrirTelaProduto, (object parameter) => { return true; });
            AbrirTelaFornecedorComando = new RelayCommand(AbrirTelaFornecedor, (object parameter) => { return true; });
            AbrirTelaMarcaComando = new RelayCommand(AbrirTelaMarca, (object parameter) => { return true; });
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
    }
}
