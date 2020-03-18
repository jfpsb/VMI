using System;
using System.Collections.ObjectModel;
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
        private string _pesquisaProdutoTxtBox;
        private DAOProduto _daoProduto;
        private DAOContagemProduto _daoContagemProduto;
        private int _quantidade;
        private bool _isTxtPesquisaFocused;
        private ObservableCollection<ProdutoModel> _produtos;
        private ObservableCollection<ContagemProdutoModel> _contagens;

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
            _daoContagemProduto = new DAOContagemProduto(_session);
            Quantidade = 1;
            GetProdutos();
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
            adicionarContagemProduto.ShowDialog();
        }

        private void InserirContagem(object parameter)
        {
            ContagemProdutoModel contagemProduto = new ContagemProdutoModel();
            contagemProduto.Id = DateTime.Now.Ticks;
            contagemProduto.Contagem = Contagem;
            contagemProduto.Produto = Produto;
            contagemProduto.Quant = Quantidade;

            var result = _daoContagemProduto.Inserir(contagemProduto);

            if (result.Result)
            {
                _session.Refresh(Contagem);
                Contagens = new ObservableCollection<ContagemProdutoModel>(Contagem.Contagens);
                PesquisaProdutoTxtBox = string.Empty;
                Quantidade = 1;
                IsTxtPesquisaFocused = true;
            }
        }

        private void RemoverContagemProduto(object p)
        {
            var result = _daoContagemProduto.Deletar(ContagemProduto);

            if (result.Result)
            {
                _session.Refresh(Contagem);
                Contagens = new ObservableCollection<ContagemProdutoModel>(Contagem.Contagens);
            }
        }

        private void AbrirEditarProduto(object p)
        {
            EditarProduto editar = new EditarProduto(ContagemProduto.Produto.Cod_Barra);
            editar.ShowDialog();
        }

        private async void GetProdutos()
        {
            Produtos = new ObservableCollection<ProdutoModel>(await _daoProduto.ListarPorDescricaoCodigoDeBarra(PesquisaProdutoTxtBox));
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

        public string PesquisaProdutoTxtBox
        {
            get
            {
                return _pesquisaProdutoTxtBox;
            }

            set
            {
                _pesquisaProdutoTxtBox = value;
                OnPropertyChanged("PesquisaProdutoTxtBox");
                IsTxtPesquisaFocused = false;
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

        public bool IsTxtPesquisaFocused
        {
            get
            {
                return _isTxtPesquisaFocused;
            }

            set
            {
                _isTxtPesquisaFocused = value;
                OnPropertyChanged("IsTxtPesquisaFocused");
            }
        }
    }
}
