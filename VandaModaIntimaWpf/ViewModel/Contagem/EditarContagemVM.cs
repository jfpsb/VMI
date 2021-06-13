using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;
using ContagemProdutoModel = VandaModaIntimaWpf.Model.ContagemProduto;
using VandaModaIntimaWpf.View.Produto;
using NHibernate;
using VandaModaIntimaWpf.ViewModel.Produto;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class EditarContagemVM : CadastrarContagemVM
    {
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

        public EditarContagemVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            viewModelStrategy = new EditarContagemVMStrategy();
            AbrirAdicionarContagemProdutoComando = new RelayCommand(AbrirAdicionarContagemProduto);
            InserirContagemComando = new RelayCommand(InserirContagem);
            RemoverContagemProdutoComando = new RelayCommand(RemoverContagemProduto);
            AbrirEditarProdutoComando = new RelayCommand(AbrirEditarProduto);
            _daoProduto = new DAOProduto(_session);
            _daoContagemProduto = new DAOContagemProduto(_session);
            Contagens = new ObservableCollection<ContagemProdutoModel>(Entidade.Contagens);
            Quantidade = 1;
            GetProdutos();
        }
        private void AbrirAdicionarContagemProduto(object parameter)
        {
            EditarContagemVMJanela.AbrirAdicionarContagemProduto(this);
        }
        private async void InserirContagem(object parameter)
        {
            ContagemProdutoModel contagemProduto = new ContagemProdutoModel
            {
                Id = DateTime.Now.Ticks,
                Contagem = Entidade,
                Produto = Produto,
                Quant = Quantidade
            };

            _result = await _daoContagemProduto.Inserir(contagemProduto);

            if (_result)
            {
                _session.Refresh(Entidade);
                Contagens = new ObservableCollection<ContagemProdutoModel>(Entidade.Contagens);
                PesquisaProdutoTxtBox = string.Empty;
                Quantidade = 1;
                IsTxtPesquisaFocused = true;
            }
        }

        private async void RemoverContagemProduto(object parameter)
        {
            ContagemProduto.Deletado = true;
            _result = await _daoContagemProduto.Atualizar(ContagemProduto);

            if (_result)
            {
                _session.Refresh(Entidade);
                Contagens = new ObservableCollection<ContagemProdutoModel>(Entidade.Contagens);
            }
        }

        private void AbrirEditarProduto(object p)
        {
            EditarProdutoVM editarProdutoViewModel = new EditarProdutoVM(_session, ContagemProduto.Produto, new MessageBoxService());
            SalvarProduto editar = new SalvarProduto() { DataContext = editarProdutoViewModel };
            editar.ShowDialog();
        }

        private async void GetProdutos()
        {
            Produtos = new ObservableCollection<ProdutoModel>(await _daoProduto.ListarPorDescricaoCodigoDeBarra(PesquisaProdutoTxtBox));
        }

        public ProdutoModel Produto
        {
            get => _produto;
            set
            {
                _produto = value;
                OnPropertyChanged("Produto");
            }
        }

        public string PesquisaProdutoTxtBox
        {
            get => _pesquisaProdutoTxtBox;
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
            get => _produtos;
            set
            {
                _produtos = value;
                OnPropertyChanged("Produtos");
            }
        }

        public int Quantidade
        {
            get => _quantidade;
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
