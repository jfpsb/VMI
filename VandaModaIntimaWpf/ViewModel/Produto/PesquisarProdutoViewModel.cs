using System.Collections.Generic;
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
    class PesquisarProdutoViewModel : ObservableObject, IPesquisarViewModel
    {
        private ProdutoModel produto;
        private ProdutoCampoMarcado produtoSelecionado;
        private ObservableCollection<ProdutoCampoMarcado> produtos;
        private string termoPesquisa;
        private int pesquisarPor;
        private Visibility visibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        public ICommand AbrirCadastrarComando { get; set; }
        public ICommand AbrirApagarComando { get; set; }
        public ICommand AbrirEditarComando { get; set; }
        public ICommand ChecarItensMarcadosComando { get; set; }
        public PesquisarProdutoViewModel()
        {
            produto = new ProdutoModel();
            PropertyChanged += ProdutoViewModel_PropertyChanged;

            AbrirCadastrarComando = new RelayCommand(AbrirCadastrarNovo, IsCommandButtonEnabled);
            AbrirApagarComando = new RelayCommand(AbrirApagarMsgBox, IsCommandButtonEnabled);
            AbrirEditarComando = new RelayCommand(AbrirEditar, IsCommandButtonEnabled);
            ChecarItensMarcadosComando = new RelayCommand(ChecarItensMarcados, IsCommandButtonEnabled);

            //Seleciona o index da combobox e por padrão realiza a pesquisa ao atualizar a propriedade
            //Lista todos os produtos ao abrir tela porque texto está vazio
            PesquisarPor = 0;
        }

        public void AbrirCadastrarNovo(object parameter)
        {
            CadastrarProduto cadastrar = new CadastrarProduto();
            cadastrar.ShowDialog();

            OnPropertyChanged("TermoPesquisa"); //Realiza pesquisa se mudar seleção de combobox
        }

        public void AbrirApagarMsgBox(object parameter)
        {
            var result = MessageBox.Show("Tem Certeza Que Deseja Apagar o Produto?", "Apagar " + produtoSelecionado.Produto.Descricao + "?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool deletado = produtoSelecionado.Produto.Deletar();

                if (deletado)
                {
                    MessageBox.Show("Produto " + produtoSelecionado.Produto.Descricao + " Foi Deletado Com Sucesso", "Deletado Com Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnPropertyChanged("TermoPesquisa");
                }
                else
                {
                    MessageBox.Show("Produto Não Foi Deletado", "Deletado Com Sucesso", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void AbrirEditar(object parameter)
        {
            ProdutoModel produtoBkp = (ProdutoModel)produtoSelecionado.Produto.Clone();

            EditarProduto editar = new EditarProduto(produtoSelecionado.Produto.Cod_Barra);
            var result = editar.ShowDialog();

            if (result.HasValue && result == true)
            {
                OnPropertyChanged("TermoPesquisa");
            }
            else
            {
                produtoSelecionado.Produto.Descricao = produtoBkp.Descricao;
                produtoSelecionado.Produto.Preco = produtoBkp.Preco;
                produtoSelecionado.Produto.Fornecedor = produtoBkp.Fornecedor;
                produtoSelecionado.Produto.Marca = produtoBkp.Marca;
                produtoSelecionado.Produto.Codigos = produtoBkp.Codigos;
            }
        }

        public void ChecarItensMarcados(object parameter)
        {
            int marcados = 0;

            foreach (ProdutoCampoMarcado pm in produtos)
            {
                if (pm.IsChecked)
                    marcados++;
            }

            if (marcados > 1)
                VisibilidadeBotaoApagarSelecionado = Visibility.Visible;
            else
                VisibilidadeBotaoApagarSelecionado = Visibility.Collapsed;
        }

        public bool IsCommandButtonEnabled(object parameter)
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

        public ObservableCollection<ProdutoCampoMarcado> Produtos
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

        public ProdutoCampoMarcado ProdutoSelecionado
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

        public Visibility VisibilidadeBotaoApagarSelecionado
        {
            get { return visibilidadeBotaoApagarSelecionado; }
            set
            {
                visibilidadeBotaoApagarSelecionado = value;
                OnPropertyChanged("VisibilidadeBotaoApagarSelecionado");
            }
        }

        public string ProdutoSelecionadoDescricao
        {
            get { return produtoSelecionado.Produto.Descricao.ToUpper(); }
        }

        public void GetItems(string termo)
        {
            switch (pesquisarPor)
            {
                case 0:
                    Produtos = new ObservableCollection<ProdutoCampoMarcado>(ProdutoCampoMarcado.ConverterIList(produto.ListarPorDescricao(termo)));
                    break;
                case 1:
                    Produtos = new ObservableCollection<ProdutoCampoMarcado>(ProdutoCampoMarcado.ConverterIList(produto.ListarPorCodigoDeBarra(termo)));
                    break;
                case 2:
                    Produtos = new ObservableCollection<ProdutoCampoMarcado>(ProdutoCampoMarcado.ConverterIList(produto.ListarPorFornecedor(termo)));
                    break;
                case 3:
                    Produtos = new ObservableCollection<ProdutoCampoMarcado>(ProdutoCampoMarcado.ConverterIList(produto.ListarPorMarca(termo)));
                    break;
            }
        }

        private void ProdutoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TermoPesquisa":
                    GetItems(termoPesquisa);
                    break;
            }
        }

        public void DisposeSession()
        {
            SessionProvider.FechaSession();
        }

        public class ProdutoCampoMarcado : ObservableObject
        {
            private ProdutoModel produto;
            private bool isChecked = false;
            public ProdutoCampoMarcado(ProdutoModel produto)
            {
                this.produto = produto;
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

            public bool IsChecked
            {
                get { return isChecked; }
                set
                {
                    isChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }

            public static IList<ProdutoCampoMarcado> ConverterIList(IList<ProdutoModel> produtos)
            {
                IList<ProdutoCampoMarcado> lista = new List<ProdutoCampoMarcado>();

                foreach (ProdutoModel produto in produtos)
                {
                    ProdutoCampoMarcado pm = new ProdutoCampoMarcado(produto);
                    lista.Add(pm);
                }

                return lista;
            }
        }
    }
}
