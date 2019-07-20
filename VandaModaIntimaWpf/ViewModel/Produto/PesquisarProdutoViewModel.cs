using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class PesquisarProdutoViewModel : ObservableObject
    {
        private ProdutoModel produto;
        private ProdutoModel produtoSelecionado;
        private ObservableCollection<ProdutoModel> produtos;
        private string termoPesquisa;
        private int pesquisarPor;

        public PesquisarProdutoViewModel()
        {
            produto = new ProdutoModel();
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

        public ProdutoModel Produto
        {
            get { return produto; }
            set
            {
                produto = value;
                OnPropertyChanged("Produto");
            }
        }

        public ProdutoModel ProdutoSelecionado
        {
            get { return produtoSelecionado; }
            set
            {
                produtoSelecionado = value;
                OnPropertyChanged("ProdutoSelecionado");
                OnPropertyChanged("ProdutoSelecionadoDescricao");
            }
        }

        public string ProdutoSelecionadoDescricao
        {
            get { return produtoSelecionado.Descricao.ToUpper(); }
        }

        private void GetProdutos(string termo)
        {
            switch (pesquisarPor)
            {
                case 0:
                    Produtos = new ObservableCollection<ProdutoModel>(produto.ListarPorDescricao(termo));
                    break;
                case 1:
                    Produtos = new ObservableCollection<ProdutoModel>(produto.ListarPorCodigoDeBarra(termo));
                    break;
                case 2:
                    Produtos = new ObservableCollection<ProdutoModel>(produto.ListarPorFornecedor(termo));
                    break;
                case 3:
                    Produtos = new ObservableCollection<ProdutoModel>(produto.ListarPorMarca(termo));
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
            produto.Dispose();
        }
    }
}
