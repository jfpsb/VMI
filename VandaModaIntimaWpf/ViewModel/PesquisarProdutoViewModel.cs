using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.Servico;

namespace VandaModaIntimaWpf.ViewModel
{
    class PesquisarProdutoViewModel : ObservableObject
    {
        private ProdutoServico produtoServico;
        private ObservableCollection<Produto> produtos;
        private string termoPesquisa;
        private int pesquisarPor;

        public PesquisarProdutoViewModel()
        {
            produtoServico = new ProdutoServico();
            PropertyChanged += ProdutoViewModel_PropertyChanged;

            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        public int PesquisarPor
        {
            get { return pesquisarPor; }
            set
            {
                pesquisarPor = value;
                OnPropertyChanged("TermoPesquisa"); //Realiza pesquisa se mudar seleção de combobox
            }
        }

        public ObservableCollection<Produto> Produtos
        {
            get { return produtos; }
            set
            {
                produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public string TermoPesquisa
        {
            get { return termoPesquisa; }
            set
            {
                termoPesquisa = value;
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public void GetProdutos(string termo)
        {
            switch (pesquisarPor)
            {
                case 0:
                    Produtos = new ObservableCollection<Produto>(produtoServico.ListarPorDescricao(termo));
                    break;
                case 1:
                    Produtos = new ObservableCollection<Produto>(produtoServico.ListarPorCodigoDeBarra(termo));
                    break;
                case 2:
                    Produtos = new ObservableCollection<Produto>(produtoServico.ListarPorFornecedor(termo));
                    break;
                case 3:
                    Produtos = new ObservableCollection<Produto>(produtoServico.ListarPorMarca(termo));
                    break;
            }
        }

        private void ProdutoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TermoPesquisa":
                    GetProdutos(termoPesquisa);
                    break;
            }
        }

        public void DisposeServico()
        {
            produtoServico.DisposeDAO();
        }
    }
}
