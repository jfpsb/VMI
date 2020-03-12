using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View.Contagem;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;
using ContagemProdutoModel = VandaModaIntimaWpf.Model.ContagemProduto;
using VandaModaIntimaWpf.View.Produto;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class EditarContagemViewModel : CadastrarContagemViewModel, IEditarViewModel
    {
        private bool IsEditted = false;
        private ProdutoModel _produto;
        private ContagemProdutoModel _contagemProduto;
        private string _pesquisaProdutoComboBox;
        private DAOProduto _daoProduto;
        private IList<ProdutoModel> _produtosFromDatabase;
        private int _quantidade;
        private ObservableCollection<ProdutoModel> _produtos;
        private ObservableCollection<ContagemProdutoModel> _contagens;

        public object _lock;
        public ICommand AbrirAdicionarContagemProdutoComando { get; set; }
        public ICommand InserirContagemProdutoComando { get; set; }
        public ICommand InserirContagemComando { get; set; }
        public ICommand RemoverContagemProdutoComando { get; set; }
        public ICommand AbrirEditarProdutoComando { get; set; }

        public EditarContagemViewModel() : base()
        {
            AbrirAdicionarContagemProdutoComando = new RelayCommand(AbrirAdicionarContagemProduto);
            InserirContagemComando = new RelayCommand(InserirContagem);
            RemoverContagemProdutoComando = new RelayCommand(RemoverContagemProduto);
            AbrirEditarProdutoComando = new RelayCommand(AbrirEditarProduto);
            _daoProduto = new DAOProduto(_session);
            Quantidade = 1;
            _lock = new object();
            GetProdutosFromDatabase();
        }

        public override async void Salvar(object parameter)
        {
            var result = IsEditted = await _daoContagem.Atualizar(Contagem);

            if (result)
            {
                await SetStatusBarSucesso("Contagem Atualizada Com Sucesso");
            }
            else
            {
                SetStatusBarErro("Erro ao Atualizar Contagem");
            }
        }

        public bool EdicaoComSucesso()
        {
            return IsEditted;
        }

        public async void PassaId(object Id)
        {
            Contagem = await _session.LoadAsync<ContagemModel>(Id);
            Contagens = new ObservableCollection<ContagemProdutoModel>(Contagem.Contagens);
        }

        private void AbrirAdicionarContagemProduto(object parameter)
        {
            AdicionarContagemProduto adicionarContagemProduto = new AdicionarContagemProduto();
            adicionarContagemProduto.DataContext = this;
            BindingOperations.EnableCollectionSynchronization(Contagens, _lock);
            adicionarContagemProduto.ShowDialog();
        }

        private void InserirContagem(object parameter)
        {
            lock (_lock)
            {
                ContagemProdutoModel contagemProduto = new ContagemProdutoModel();
                contagemProduto.Id = DateTime.Now.Ticks;
                contagemProduto.Contagem = Contagem;
                contagemProduto.Produto = Produto;
                contagemProduto.Quant = Quantidade;

                Contagens.Add(contagemProduto);

                Contagem.Contagens.Clear();

                foreach(ContagemProdutoModel c in Contagens)
                {
                    Contagem.Contagens.Add(c);
                }
            }
        }

        private void RemoverContagemProduto(object p)
        {
            lock (_lock)
            {
                Contagens.Remove(ContagemProduto);

                Contagem.Contagens.Clear();

                foreach (ContagemProdutoModel c in Contagens)
                {
                    Contagem.Contagens.Add(c);
                }
            }
        }

        private void AbrirEditarProduto(object p)
        {
            EditarProduto editar = new EditarProduto(ContagemProduto.Produto.Cod_Barra);
            editar.ShowDialog();
        }

        private void GetProdutos()
        {
            IList<ProdutoModel> lista = _produtosFromDatabase.Where(w => w.Codigos.Contains(PesquisaProdutoComboBox)
                                                                        || w.Cod_Barra.Contains(PesquisaProdutoComboBox)
                                                                        || w.Descricao.Contains(PesquisaProdutoComboBox)).ToList();
            Produtos = new ObservableCollection<ProdutoModel>(lista);
        }

        private async void GetProdutosFromDatabase()
        {
            _produtosFromDatabase = await _daoProduto.Listar<ProdutoModel>();
        }

        public ProdutoModel Produto
        {
            get
            {
                return _produto;
            }

            set
            {
                _produto = value;
                OnPropertyChanged("Produto");
            }
        }

        public string PesquisaProdutoComboBox
        {
            get
            {
                return _pesquisaProdutoComboBox;
            }

            set
            {
                _pesquisaProdutoComboBox = value;
                OnPropertyChanged("PesquisaProdutoComboBox");
                GetProdutos();
            }
        }

        public ObservableCollection<ProdutoModel> Produtos
        {
            get
            {
                return _produtos;
            }

            set
            {
                _produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public int Quantidade
        {
            get
            {
                return _quantidade;
            }

            set
            {
                _quantidade = value;
                OnPropertyChanged("Quantidade");
            }
        }

        public ObservableCollection<ContagemProdutoModel> Contagens
        {
            get
            {
                return _contagens;
            }

            set
            {
                _contagens = value;
                OnPropertyChanged("Contagens");
            }
        }

        public ContagemProdutoModel ContagemProduto
        {
            get
            {
                return _contagemProduto;
            }

            set
            {
                _contagemProduto = value;
                OnPropertyChanged("ContagemProduto");
            }
        }
    }
}
