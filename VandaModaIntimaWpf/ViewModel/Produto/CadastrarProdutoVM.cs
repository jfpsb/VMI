using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class CadastrarProdutoVM : ACadastrarViewModel<ProdutoModel>
    {
        protected DAOMarca daoMarca;
        protected DAOFornecedor daoFornecedor;
        protected DAOTipoGrade daoTipoGrade;
        protected DAOGrade daoGrade;
        protected DAOProdutoGrade daoProdutoGrade;

        private string _codigoFornecedor;
        private TipoGrade _tipoGrade;
        private Grade _subGrade;
        private ObservableCollection<Grade> _subGrades;
        private ObservableCollection<Grade> _grades;
        private ObservableCollection<TipoGrade> tiposGrade;
        private ProdutoGrade _produtoGrade;
        private ObservableCollection<ProdutoGrade> _produtoGrades;

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }
        public ICommand InserirFormacaoGradeComando { get; set; }
        public CadastrarProdutoVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            cadastrarViewModelStrategy = new CadastrarProdutoMsgVMStrategy();
            daoEntidade = new DAOProduto(_session);
            daoMarca = new DAOMarca(_session);
            daoFornecedor = new DAOFornecedor(_session);
            daoTipoGrade = new DAOTipoGrade(_session);
            daoProdutoGrade = new DAOProdutoGrade(_session);
            daoGrade = new DAOGrade(_session);
            Entidade = new ProdutoModel();
            ProdutoGrade = new ProdutoGrade();
            SubGrades = new ObservableCollection<Grade>();

            InserirFormacaoGradeComando = new RelayCommand(InserirFormacaoGrade);

            Entidade.PropertyChanged += Entidade_PropertyChanged;
            PropertyChanged += GetGrades;

            GetFornecedores();
            GetMarcas();
            GetTiposGrade();
        }

        private void InserirFormacaoGrade(object obj)
        {
            SubGrades.Add(SubGrade);
        }

        private async void GetGrades(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TipoGrade":
                    Grades = new ObservableCollection<Grade>(await daoGrade.ListarPorTipoGrade(TipoGrade));
                    break;
                case "Grades":
                    SubGrade = Grades[0];
                    break;
                case "TiposGrade":
                    TipoGrade = TiposGrade[0];
                    break;
            }
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.CodBarra) || string.IsNullOrEmpty(Entidade.Descricao))
            {
                return false;
            }

            if (Entidade.Preco.ToString().Equals(string.Empty) || Entidade.Preco == 0)
            {
                return false;
            }

            if (!IsEnabled)
                return false;

            return true;
        }
        public override void ResetaPropriedades()
        {
            Entidade = new ProdutoModel();
            Entidade.CodBarra = Entidade.Descricao = string.Empty;
            Entidade.Preco = 0;
            Entidade.Fornecedor = Fornecedores[0];
            Entidade.Marca = Marcas[0];
        }
        public string CodigoFornecedor
        {
            get
            {
                return _codigoFornecedor;
            }

            set
            {
                _codigoFornecedor = value;
                OnPropertyChanged("CodigoFornecedor");
            }
        }

        public TipoGrade TipoGrade
        {
            get => _tipoGrade;
            set
            {
                _tipoGrade = value;
                OnPropertyChanged("TipoGrade");
            }
        }

        public Grade SubGrade
        {
            get => _subGrade;
            set
            {
                _subGrade = value;
                OnPropertyChanged("SubGrade");
            }
        }

        public ObservableCollection<Grade> Grades
        {
            get => _grades;
            set
            {
                _grades = value;
                OnPropertyChanged("Grades");
            }
        }

        public ObservableCollection<TipoGrade> TiposGrade
        {
            get => tiposGrade;
            set
            {
                tiposGrade = value;
                OnPropertyChanged("TiposGrade");
            }
        }

        public ProdutoGrade ProdutoGrade
        {
            get => _produtoGrade;
            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrade");
            }
        }

        public ObservableCollection<ProdutoGrade> ProdutoGrades
        {
            get => _produtoGrades;
            set
            {
                _produtoGrades = value;
                OnPropertyChanged("ProdutoGrades");
            }
        }

        public ObservableCollection<Grade> SubGrades
        {
            get => _subGrades;
            set
            {
                _subGrades = value;
                OnPropertyChanged("SubGrades");
            }
        }

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<FornecedorModel>(await daoFornecedor.Listar<FornecedorModel>());
            Fornecedores.Insert(0, new FornecedorModel(GetResource.GetString("fornecedor_nao_selecionado")));
        }
        private async void GetMarcas()
        {
            Marcas = new ObservableCollection<MarcaModel>(await daoMarca.Listar<MarcaModel>());
            Marcas.Insert(0, new MarcaModel(GetResource.GetString("marca_nao_selecionada")));
        }
        private async void GetTiposGrade()
        {
            TiposGrade = new ObservableCollection<TipoGrade>(await daoTipoGrade.Listar<TipoGrade>());
        }
        public override async void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CodBarra":
                    var result = await daoEntidade.ListarPorId(Entidade.CodBarra);

                    if (result != null)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }
        protected override void ExecutarAntesCriarDocumento()
        {
            if (Entidade.Fornecedor?.Cnpj == null)
                Entidade.Fornecedor = null;

            if (Entidade.Marca != null && Entidade.Marca.Nome.Equals(GetResource.GetString("marca_nao_selecionada")))
                Entidade.Marca = null;
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
