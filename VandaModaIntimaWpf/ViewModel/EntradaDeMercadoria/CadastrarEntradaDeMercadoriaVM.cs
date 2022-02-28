using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    class CadastrarEntradaDeMercadoriaVM : ACadastrarViewModel<Model.EntradaDeMercadoria>
    {
        private string _termoPesquisaProduto;
        private DAOProduto daoProduto;
        private DAOLoja daoLoja;
        private Model.Produto _produto;
        private int _listViewZIndex;
        private int _quantidade;
        private ObservableCollection<Model.Produto> _produtosPesquisa = new ObservableCollection<Model.Produto>();
        private ObservableCollection<Model.EntradaMercadoriaProdutoGrade> _entradas = new ObservableCollection<Model.EntradaMercadoriaProdutoGrade>();
        private ObservableCollection<Model.Loja> _lojas = new ObservableCollection<Model.Loja>();
        private bool _isTxtPesquisaProdutoFocused;
        private bool _isTxtQuantidadeFocused;
        private bool _isListViewFocused;
        private string _produtoDescricao;

        public ICommand InserirProdutoDataGridComando { get; set; }
        public ICommand TxtPesquisaProdutoEnterComando { get; set; }
        public ICommand ListViewEnterComando { get; set; }

        public CadastrarEntradaDeMercadoriaVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarEntradaDeMercadoriaVMStrategy();
            daoEntidade = new DAOEntradaDeMercadoria(session);
            daoProduto = new DAOProduto(session);
            daoLoja = new DAOLoja(session);
            Entidade = new Model.EntradaDeMercadoria();

            InserirProdutoDataGridComando = new RelayCommand(InserirProdutoDataGrid);
            TxtPesquisaProdutoEnterComando = new RelayCommand(TxtPesquisaProdutoEnter);
            ListViewEnterComando = new RelayCommand(ListViewEnter);

            PropertyChanged += PesquisaProdutos;
            AntesDeInserirNoBancoDeDados += CadastrarEntradaDeMercadoriaVM_AntesDeInserirNoBancoDeDados;

            GetLojas();
            TermoPesquisaProduto = "";
            ProdutoDescricao = "";
        }

        private void CadastrarEntradaDeMercadoriaVM_AntesDeInserirNoBancoDeDados()
        {
            Entidade.Entradas = Entradas;
        }

        private void ListViewEnter(object obj)
        {
            if (Produto != null)
            {
                IsTxtPesquisaProdutoFocused = false;
                IsTxtQuantidadeFocused = true;
                IsListViewFocused = false;

                ListViewZIndex = 0;
            }
        }

        private void TxtPesquisaProdutoEnter(object obj)
        {
            if (TermoPesquisaProduto.Length > 0)
            {
                if (Produto == null)
                {
                    IsTxtPesquisaProdutoFocused = false;
                    IsTxtQuantidadeFocused = false;
                    IsListViewFocused = true;
                }
                else
                {
                    IsTxtPesquisaProdutoFocused = false;
                    IsTxtQuantidadeFocused = true;
                    IsListViewFocused = false;
                }
            }
        }

        private void InserirProdutoDataGrid(object obj)
        {
            EntradaMercadoriaProdutoGrade entradaMercadoriaProdutoGrade = new EntradaMercadoriaProdutoGrade
            {
                Entrada = Entidade,
                ProdutoGrade = Produto.Grades[0],
                Quantidade = Quantidade
            };

            Entradas.Add(entradaMercadoriaProdutoGrade);

            Quantidade = 0;
            TermoPesquisaProduto = "";
            IsTxtPesquisaProdutoFocused = true;
            IsTxtQuantidadeFocused = false;
            IsListViewFocused = false;
        }

        private async void PesquisaProdutos(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TermoPesquisaProduto"))
            {
                Produto = null;

                if (TermoPesquisaProduto.Length > 0)
                {
                    ProdutosPesquisa = new ObservableCollection<Model.Produto>(await daoProduto.ListarPorDescricaoCodigoDeBarra(TermoPesquisaProduto));
                    if (ProdutosPesquisa.Count > 0)
                        ListViewZIndex = 1;
                    else
                        ListViewZIndex = 0;
                }
                else
                {
                    ListViewZIndex = 0;
                }
            }
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            Entidade = new Model.EntradaDeMercadoria();
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (Entidade.Entradas.Count == 0)
            {
                BtnSalvarToolTip += "Ao Menos Um Produto Deve Ser Adicionado Para Cadastrar A Entrada De Mercadoria!\n";
                valido = false;
            }

            return valido;
        }

        private async void GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.ListarExcetoDeposito());
            Entidade.Loja = Lojas[0];
        }

        public string TermoPesquisaProduto
        {
            get => _termoPesquisaProduto;
            set
            {
                _termoPesquisaProduto = value;
                OnPropertyChanged("TermoPesquisaProduto");
            }
        }

        public Model.Produto Produto
        {
            get => _produto;
            set
            {
                _produto = value;
                OnPropertyChanged("Produto");
                if (value != null)
                    ProdutoDescricao = value.Descricao;
                else
                    ProdutoDescricao = "";
            }
        }

        public int ListViewZIndex
        {
            get => _listViewZIndex;
            set
            {
                _listViewZIndex = value;
                OnPropertyChanged("ListViewZIndex");
            }
        }

        public ObservableCollection<Model.Produto> ProdutosPesquisa
        {
            get => _produtosPesquisa;
            set
            {
                _produtosPesquisa = value;
                OnPropertyChanged("ProdutosPesquisa");
            }
        }

        public ObservableCollection<EntradaMercadoriaProdutoGrade> Entradas
        {
            get => _entradas;
            set
            {
                _entradas = value;
                OnPropertyChanged("ProdutoGrades");
            }
        }

        public bool IsTxtPesquisaProdutoFocused
        {
            get => _isTxtPesquisaProdutoFocused;
            set
            {
                _isTxtPesquisaProdutoFocused = value;
                OnPropertyChanged("IsTxtPesquisaProdutoFocused");
            }
        }
        public bool IsTxtQuantidadeFocused
        {
            get => _isTxtQuantidadeFocused;
            set
            {
                _isTxtQuantidadeFocused = value;
                OnPropertyChanged("IsTxtQuantidadeFocused");
            }
        }

        public bool IsListViewFocused
        {
            get => _isListViewFocused;
            set
            {
                _isListViewFocused = value;
                OnPropertyChanged("IsListViewFocused");
            }
        }

        public string ProdutoDescricao
        {
            get => _produtoDescricao;
            set
            {
                _produtoDescricao = value;
                OnPropertyChanged("ProdutoDescricao");
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

        public ObservableCollection<Model.Loja> Lojas
        {
            get => _lojas;
            set
            {
                _lojas = value;
                OnPropertyChanged("Lojas");
            }
        }
    }
}
