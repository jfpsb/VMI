using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Produto;

namespace VandaModaIntimaWpf.ViewModel
{
    class VandaModaIntimaViewModel : ObservableObject
    {
        public ICommand AbrirTelaProdutoComando { get; set; }
        public ICommand AbrirTelaFornecedorComando { get; set; }
        public VandaModaIntimaViewModel()
        {
            AbrirTelaProdutoComando = new RelayCommand(AbrirTelaProduto, CommandEnabled);
            AbrirTelaFornecedorComando = new RelayCommand(AbrirTelaFornecedor, CommandEnabled);
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
        private bool CommandEnabled(object parameter)
        {
            return true;
        }
    }
}
