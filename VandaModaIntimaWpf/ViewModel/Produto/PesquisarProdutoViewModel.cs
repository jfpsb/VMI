using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.Servico;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class PesquisarProdutoViewModel : ObservableObject
    {
        private ProdutoServico produtoServico;
        private ObservableCollection<ProdutoModel> produtos;
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

        public ObservableCollection<ProdutoModel> Produtos
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

        private void GetProdutos(string termo)
        {
            switch (pesquisarPor)
            {
                case 0:
                    Produtos = new ObservableCollection<ProdutoModel>(produtoServico.ListarPorDescricao(termo));
                    break;
                case 1:
                    Produtos = new ObservableCollection<ProdutoModel>(produtoServico.ListarPorCodigoDeBarra(termo));
                    break;
                case 2:
                    Produtos = new ObservableCollection<ProdutoModel>(produtoServico.ListarPorFornecedor(termo));
                    break;
                case 3:
                    Produtos = new ObservableCollection<ProdutoModel>(produtoServico.ListarPorMarca(termo));
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
