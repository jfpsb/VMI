using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.View.Produto;
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

        public ICommand AbrirCadastrarComando { get; set; }
        public ICommand AbrirApagarComando { get; set; }
        public PesquisarProdutoViewModel()
        {
            produto = new ProdutoModel();
            PropertyChanged += ProdutoViewModel_PropertyChanged;

            AbrirCadastrarComando = new RelayCommand(AbrirCadastrarNovo, IsCommandButtonEnabled);
            AbrirApagarComando = new RelayCommand(AbrirApagarMsgBox, IsCommandButtonEnabled);

            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        public void AbrirCadastrarNovo(object parameter)
        {
            int aux = pesquisarPor;
            CadastrarProduto cadastrar = new CadastrarProduto();
            cadastrar.ShowDialog();
            pesquisarPor = aux;
        }

        public void AbrirApagarMsgBox(object parameter)
        {
            var result = MessageBox.Show("Tem Certeza Que Deseja Apagar o Produto?", "Apagar " + produtoSelecionado.Descricao + "?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                bool deletado = produtoSelecionado.Deletar();

                if(deletado)
                {
                    MessageBox.Show("Produto " + produtoSelecionado.Descricao + " Foi Deletado Com Sucesso", "Deletado Com Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    MessageBox.Show("Produto Não Foi Deletado", "Deletado Com Sucesso", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsCommandButtonEnabled(object parameter)
        {
            return true;
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
                if (produtoSelecionado != null)
                {
                    OnPropertyChanged("ProdutoSelecionado");
                    OnPropertyChanged("ProdutoSelecionadoDescricao");
                }
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

        public void DisposeSession()
        {
            SessionProvider.FechaSession();
        }
    }
}
