using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.ViewModel.Services.Concretos;
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
        private Model.TipoGrade _tipoGrade;
        private Model.Grade _grade; // Guarda a grade atualmente selecionada na ComboxBox de Grades
        private ObservableCollection<Model.Grade> _grades; // Guarda a lista de grades presentes no DataGrid de grade em formação
        private ObservableCollection<Model.Grade> _gradesComboBox;
        private ObservableCollection<Model.TipoGrade> tiposGrade; // Coleção usada na ComboBox de Tipo de Grade
        private ProdutoGrade _produtoGrade; // Guarda ProdutoGrade sendo formada
        private ObservableCollection<ProdutoGrade> _produtoGrades; // Guarda listagem de Grades do Produto já completamente formadas
        private AbrePelaTelaCadastroDeProduto abrePelaTelaCadastroDeProduto;

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }
        public ICommand InserirFormacaoGradeComando { get; set; }
        public ICommand InserirFormacaoAtualGradeComando { get; set; }
        public ICommand AbreTelaCadastrarTipoGradeComando { get; set; }
        public ICommand AbreTelaCadastrarGradeComando { get; set; }
        public CadastrarProdutoVM(ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            viewModelStrategy = new CadastrarProdutoVMStrategy();
            abrePelaTelaCadastroDeProduto = new AbrePelaTelaCadastroDeProduto();
            daoEntidade = new DAOProduto(_session);
            daoMarca = new DAOMarca(_session);
            daoFornecedor = new DAOFornecedor(_session);
            daoTipoGrade = new DAOTipoGrade(_session);
            daoProdutoGrade = new DAOProdutoGrade(_session);
            daoGrade = new DAOGrade(_session);
            Entidade = new ProdutoModel();
            ProdutoGrade = new ProdutoGrade();
            ProdutoGrades = new ObservableCollection<ProdutoGrade>();
            Grades = new ObservableCollection<Model.Grade>();

            InserirFormacaoGradeComando = new RelayCommand(InserirFormacaoGrade);
            InserirFormacaoAtualGradeComando = new RelayCommand(InserirFormacaoAtualGrade, ValidaInserirFormacaoAtualGrade);
            AbreTelaCadastrarGradeComando = new RelayCommand(AbreTelaCadastrarGrade);
            AbreTelaCadastrarTipoGradeComando = new RelayCommand(AbreTelaCadastrarTipoGrade);

            Entidade.PropertyChanged += ChecaPropriedadesProduto;
            PropertyChanged += GetGrades;

            AntesDeInserirNoBancoDeDados += ConfiguraProdutoAntesDeInserir;
            AntesDeInserirNoBancoDeDados += AdicionaGradesEmEntidade;

            AposInserirNoBancoDeDados += LimpaGrades;

            GetFornecedores();
            GetMarcas();
            GetTiposGrade();
        }

        private void ConfiguraProdutoAntesDeInserir()
        {
            if (Entidade.Fornecedor?.Cnpj == null)
                Entidade.Fornecedor = null;

            if (Entidade.Marca != null && Entidade.Marca.Nome.Equals(GetResource.GetString("marca_nao_selecionada")))
                Entidade.Marca = null;
        }

        private void AbreTelaCadastrarTipoGrade(object obj)
        {
            abrePelaTelaCadastroDeProduto.AbrirTelaCadastrarTipoGrade(_session);
            OnPropertyChanged("TiposGrade");
        }

        private void AbreTelaCadastrarGrade(object obj)
        {
            abrePelaTelaCadastroDeProduto.AbrirTelaCadastrarGrade(_session);
        }

        private void LimpaGrades(AposInserirBDEventArgs e)
        {
            if (e.IdentificadorEntidade != null && !e.IssoEUmUpdate)
            {
                Grades.Clear();
                ProdutoGrades.Clear();
            }
        }

        private void AdicionaGradesEmEntidade()
        {
            Entidade.Grades.Clear();
            foreach (var g in ProdutoGrades)
            {
                Entidade.Grades.Add(g);
            }
        }

        private bool ValidaInserirFormacaoAtualGrade(object arg)
        {
            if (string.IsNullOrEmpty(ProdutoGrade.CodBarra))
                return false;

            if (string.IsNullOrEmpty(ProdutoGrade.Preco.ToString()) || ProdutoGrade.Preco <= 0.0)
                return false;

            if (Grades.Count == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Insere a formação atual de grade na listagem de grade formada
        /// </summary>
        /// <param name="obj"></param>
        private void InserirFormacaoAtualGrade(object obj)
        {
            ProdutoGrade.Produto = Entidade;

            foreach (var grade in Grades)
            {
                SubGrade subGrade = new SubGrade
                {
                    ProdutoGrade = ProdutoGrade,
                    Grade = grade
                };
                ProdutoGrade.SubGrades.Add(subGrade);
            }

            Grades.Clear();

            ProdutoGrades.Add(ProdutoGrade);
            Entidade.Grades.Add(ProdutoGrade);

            // Reseta ProdutoGrade
            ProdutoGrade = new ProdutoGrade()
            {
                Produto = Entidade,
                Preco = Entidade.Preco
            };
        }

        /// <summary>
        /// Insere a grade na lista de grade ainda em formação
        /// </summary>
        /// <param name="obj"></param>
        private void InserirFormacaoGrade(object obj)
        {
            Grades.Add(Grade);
        }

        private async void GetGrades(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TipoGrade":
                    GradesComboBox = new ObservableCollection<Model.Grade>(await daoGrade.ListarPorTipoGrade(TipoGrade));
                    break;
                case "GradesComboBox":
                    Grade = GradesComboBox[0];
                    break;
                case "TiposGrade":
                    TipoGrade = TiposGrade[0];
                    break;
            }
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.CodBarra?.Trim()) || string.IsNullOrEmpty(Entidade.Descricao))
            {
                SetStatusBarErro("Código ou Descrição Não Podem Ser Vazios");
                return false;
            }

            if (Entidade.Preco.ToString().Equals(string.Empty) || Entidade.Preco <= 0)
            {
                SetStatusBarErro("Preço Não Pode Ser Vazio Ou Inválido");
                return false;
            }

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

        public Model.TipoGrade TipoGrade
        {
            get => _tipoGrade;
            set
            {
                _tipoGrade = value;
                OnPropertyChanged("TipoGrade");
            }
        }

        public Model.Grade Grade
        {
            get => _grade;
            set
            {
                _grade = value;
                OnPropertyChanged("Grade");
            }
        }

        public ObservableCollection<Model.Grade> GradesComboBox
        {
            get => _gradesComboBox;
            set
            {
                _gradesComboBox = value;
                OnPropertyChanged("GradesComboBox");
            }
        }

        public ObservableCollection<Model.TipoGrade> TiposGrade
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

        public ObservableCollection<Model.Grade> Grades
        {
            get => _grades;
            set
            {
                _grades = value;
                OnPropertyChanged("Grades");
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
            TiposGrade = new ObservableCollection<Model.TipoGrade>(await daoTipoGrade.Listar<Model.TipoGrade>());
        }
        public async void ChecaPropriedadesProduto(object sender, PropertyChangedEventArgs e)
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

                    ProdutoGrade.Produto = Entidade;

                    break;
                case "Preco":
                    ProdutoGrade.Preco = Entidade.Preco;
                    break;
            }
        }
    }
}
