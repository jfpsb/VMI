using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.Fornecedor;
using VandaModaIntimaWpf.View.Marca;
using VandaModaIntimaWpf.ViewModel.Fornecedor;
using VandaModaIntimaWpf.ViewModel.Marca;
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
        protected DAOLoja daoLoja;
        protected DAO<Model.ProdutoGrade> daoProdutoGrade;
        protected DAOHistoricoProdutoGrade daoComposicaoPreco;

        private string _codigoFornecedor;
        private Model.TipoGrade _tipoGrade;
        private Model.Grade _grade; // Guarda a grade atualmente selecionada na ComboxBox de Grades
        private ObservableCollection<Model.Grade> _grades; // Guarda a lista de grades presentes no DataGrid de grade em formação
        private ObservableCollection<Model.Grade> _gradesComboBox;
        private ObservableCollection<Model.TipoGrade> tiposGrade; // Coleção usada na ComboBox de Tipo de Grade
        private ProdutoGrade _produtoGrade; // Guarda ProdutoGrade sendo formada
        private ProdutoGrade _produtoGradeSelecionada;
        private ObservableCollection<ProdutoGrade> _produtoGrades; // Guarda listagem de Grades do Produto já completamente formadas
        private AbrePelaTelaCadastroDeProduto abrePelaTelaCadastroDeProduto;
        private ObservableCollection<FornecedorModel> _fornecedores;
        private ObservableCollection<MarcaModel> _marcas;
        private ObservableCollection<Model.ComposicaoPreco> _composicaoPrecos;
        private ProdutoGrade _produtoGradeComposicaoPreco;
        private double _frete;
        private double _precoCompra;
        private double _precoVenda;
        private double _mediaCustoTotal;
        private double _mediaMargemContribuicao;
        private double _mediaLucro;
        private bool _aplicaIcms = true;
        private bool _aplicaTodasProdutoGrade;
        private ObservableCollection<HistoricoProdutoGrade> _historicoProdutoGrade = new ObservableCollection<HistoricoProdutoGrade>();

        private IList<Model.Loja> matrizes;

        public ICommand InserirFormacaoGradeComando { get; set; }
        public ICommand InserirFormacaoAtualGradeComando { get; set; }
        public ICommand AbreTelaCadastrarTipoGradeComando { get; set; }
        public ICommand AbreTelaCadastrarGradeComando { get; set; }
        public ICommand CadastrarFornecedorOnlineComando { get; set; }
        public ICommand CadastrarFornecedorManualmenteComando { get; set; }
        public ICommand CadastrarMarcaComando { get; set; }
        public ICommand CopiarCodBarraComando { get; set; }
        public ICommand SalvaComposicaoPrecoComando { get; set; }
        public CadastrarProdutoVM(ISession session, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            viewModelStrategy = new CadastrarProdutoVMStrategy();
            abrePelaTelaCadastroDeProduto = new AbrePelaTelaCadastroDeProduto();
            daoEntidade = new DAOProduto(_session);
            daoMarca = new DAOMarca(_session);
            daoFornecedor = new DAOFornecedor(_session);
            daoTipoGrade = new DAOTipoGrade(_session);
            daoProdutoGrade = new DAO<Model.ProdutoGrade>(_session);
            daoGrade = new DAOGrade(_session);
            daoLoja = new DAOLoja(_session);
            daoComposicaoPreco = new DAOHistoricoProdutoGrade(_session);
            Entidade = new ProdutoModel();
            ProdutoGrade = new ProdutoGrade();
            ProdutoGrades = new ObservableCollection<ProdutoGrade>();
            Grades = new ObservableCollection<Model.Grade>();

            InserirFormacaoGradeComando = new RelayCommand(InserirFormacaoGrade);
            InserirFormacaoAtualGradeComando = new RelayCommand(InserirFormacaoAtualGrade, ValidaInserirFormacaoAtualGrade);
            AbreTelaCadastrarGradeComando = new RelayCommand(AbreTelaCadastrarGrade);
            AbreTelaCadastrarTipoGradeComando = new RelayCommand(AbreTelaCadastrarTipoGrade);
            CadastrarFornecedorOnlineComando = new RelayCommand(CadastrarFornecedorOnline);
            CadastrarFornecedorManualmenteComando = new RelayCommand(CadastrarFornecedorManualmente);
            CadastrarMarcaComando = new RelayCommand(CadastrarMarca);
            CopiarCodBarraComando = new RelayCommand(CopiarCodBarra);
            SalvaComposicaoPrecoComando = new RelayCommand(SalvaComposicaoPreco);

            PropertyChanged += GetGrades;
            PropertyChanged += FreteAlterado;
            PropertyChanged += ProdutoGradeComposicaoAlterada;
            PropertyChanged += PrecoCompraAlterado;
            PropertyChanged += PrecoVendaAlterado;
            PropertyChanged += AplicaIcmsAlterado;

            AntesDeInserirNoBancoDeDados += ChecaProdutoGrades;
            AntesDeInserirNoBancoDeDados += ConfiguraProdutoAntesDeInserir;
            AntesDeInserirNoBancoDeDados += AdicionaGradesEmEntidade;

            AposInserirNoBancoDeDados += LimpaGrades;

            GetFornecedores();
            GetMarcas();
            GetTiposGrade();
            GetMatrizes();

            ComposicaoPrecos = new ObservableCollection<ComposicaoPreco>();
            ComposicaoPrecos.CollectionChanged += ComposicaoPrecos_CollectionChanged;

            CriaComposicaoPreco();
        }

        private void ChecaProdutoGrades()
        {
            if (ProdutoGrades.Count == 0)
            {
                MessageBoxService.Show("O Produto Precisa De Ao Menos Uma Grade Para Ser Cadastrado!");
                AntesInserirBDChecagem = false;
            }
        }

        private void AplicaIcmsAlterado(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AplicaIcms"))
            {
                foreach (var comp in ComposicaoPrecos)
                {
                    comp.AplicaIcms = AplicaIcms;
                }

                CalculaMediasComposicaoPrecos();
            }
        }

        private void PrecoVendaAlterado(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("PrecoVenda"))
            {
                foreach (var comp in ComposicaoPrecos)
                {
                    comp.PrecoVenda = PrecoVenda;
                }

                CalculaMediasComposicaoPrecos();
            }
        }

        private async void SalvaComposicaoPreco(object obj)
        {
            if (AplicaTodasProdutoGrade)
            {
                foreach (var pg in ProdutoGrades)
                {
                    HistoricoProdutoGrade historicoProdutoGrade = new HistoricoProdutoGrade
                    {
                        ProdutoGrade = pg,
                        Data = DateTime.Now,
                        PrecoCompra = PrecoCompra,
                        PrecoVenda = PrecoVenda,
                        CustoTotal = MediaCustoTotal,
                        Frete = Frete
                    };

                    pg.PrecoCusto = MediaCustoTotal;
                    pg.Preco = PrecoVenda;
                    pg.Historico.Add(historicoProdutoGrade);
                }

                try
                {
                    await daoProdutoGrade.InserirOuAtualizar(ProdutoGrades);
                    MessageBoxService.Show("Grades atualizadas com novos preços de custo com sucesso!", viewModelStrategy.MessageBoxCaption(), MessageBoxButton.OK, MessageBoxImage.Error);
                    HistoricoProdutoGrade = new ObservableCollection<HistoricoProdutoGrade>(ProdutoGradeComposicaoPreco.Historico);
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"Erro ao salvar composição de preço das grades.\nPara mais detalhes acesse {Log.LogBanco}\n\n{ex.Message}\n\n{ex.InnerException.Message}",
                        viewModelStrategy.MessageBoxCaption(),
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                HistoricoProdutoGrade historicoProdutoGrade = new HistoricoProdutoGrade
                {
                    ProdutoGrade = ProdutoGradeComposicaoPreco,
                    Data = DateTime.Now,
                    PrecoCompra = PrecoCompra,
                    PrecoVenda = PrecoVenda,
                    CustoTotal = MediaCustoTotal,
                    Frete = Frete
                };

                ProdutoGradeComposicaoPreco.PrecoCusto = MediaCustoTotal;
                ProdutoGradeComposicaoPreco.Preco = PrecoVenda;
                ProdutoGradeComposicaoPreco.Historico.Add(historicoProdutoGrade);

                try
                {
                    await daoProdutoGrade.Atualizar(ProdutoGradeComposicaoPreco);
                    MessageBoxService.Show("Grade atualizada com novo preço de custo com sucesso!", viewModelStrategy.MessageBoxCaption(), MessageBoxButton.OK, MessageBoxImage.Error);
                    HistoricoProdutoGrade = new ObservableCollection<HistoricoProdutoGrade>(ProdutoGradeComposicaoPreco.Historico);
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"Erro ao salvar composição de preço da grade.\nPara mais detalhes acesse {Log.LogBanco}\n\n{ex.Message}\n\n{ex.InnerException.Message}",
                        viewModelStrategy.MessageBoxCaption(),
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ComposicaoPrecos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculaMediasComposicaoPrecos();
        }

        private void CalculaMediasComposicaoPrecos()
        {
            if (ProdutoGradeComposicaoPreco != null)
            {
                MediaCustoTotal = ComposicaoPrecos.Average(a => a.CustoTotal);
                MediaLucro = ComposicaoPrecos.Average(a => a.Lucro);
                MediaMargemContribuicao = ComposicaoPrecos.Average(a => a.MargemContribuicao);
            }
        }

        private void PrecoCompraAlterado(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("PrecoCompra"))
            {
                foreach (var comp in ComposicaoPrecos)
                {
                    comp.PrecoCompra = PrecoCompra;
                }

                CalculaMediasComposicaoPrecos();
            }
        }

        private void ProdutoGradeComposicaoAlterada(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ProdutoGradeComposicaoPreco") && ProdutoGradeComposicaoPreco != null)
            {
                foreach (var comp in ComposicaoPrecos)
                {
                    comp.ProdutoGrade = ProdutoGradeComposicaoPreco;
                }

                CalculaMediasComposicaoPrecos();
                HistoricoProdutoGrade = new ObservableCollection<HistoricoProdutoGrade>(ProdutoGradeComposicaoPreco.Historico);
            }
        }

        private void FreteAlterado(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Frete"))
            {
                foreach (var comp in ComposicaoPrecos)
                {
                    comp.Frete = Frete;
                }

                CalculaMediasComposicaoPrecos();
            }
        }

        private void CriaComposicaoPreco()
        {
            foreach (var matriz in matrizes)
            {
                Model.ComposicaoPreco composicaoPreco = new ComposicaoPreco
                {
                    Loja = matriz,
                    ProdutoGrade = ProdutoGradeComposicaoPreco,
                    Data = DateTime.Now,
                    Frete = Frete
                };

                ComposicaoPrecos.Add(composicaoPreco);
            }
        }

        private void CopiarCodBarra(object obj)
        {
            Clipboard.SetText(ProdutoGradeSelecionada.CodBarra);
        }

        private void CadastrarMarca(object obj)
        {
            CadastrarMarcaVM cadastrarMarcaViewModel = new CadastrarMarcaVM(_session, new MessageBoxService(), false);
            CadastrarMarca cadastrarMarca = new CadastrarMarca()
            {
                DataContext = cadastrarMarcaViewModel
            };
            var result = cadastrarMarca.ShowDialog();

            if (result == true)
            {
                GetMarcas();
                Entidade.Marca = Marcas[0];
            }
        }

        private void CadastrarFornecedorOnline(object obj)
        {
            CadastrarFornecedorOnlineVM viewModel = new CadastrarFornecedorOnlineVM(_session, new MessageBoxService(), false);
            SalvarFornecedor cadastrarFornecedorOnline = new SalvarFornecedor
            {
                DataContext = viewModel
            };
            var result = cadastrarFornecedorOnline.ShowDialog();

            if (result == true)
            {
                GetFornecedores();
                Entidade.Fornecedor = Fornecedores[0];
            }
        }

        private void CadastrarFornecedorManualmente(object obj)
        {
            CadastrarFornecedorManualmenteVM cadastrarFornecedorManualmenteViewModel = new CadastrarFornecedorManualmenteVM(_session, new MessageBoxService(), false);
            SalvarFornecedor cadastrar = new SalvarFornecedor() { DataContext = cadastrarFornecedorManualmenteViewModel };
            var result = cadastrar.ShowDialog();

            if (result == true)
            {
                GetFornecedores();
                Entidade.Fornecedor = Fornecedores[0];
            }
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
                Produto = Entidade
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
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.CodBarra?.Trim()) || string.IsNullOrEmpty(Entidade.Descricao))
            {
                BtnSalvarToolTip += "Cód. De Barras Ou Descrição De Produto Não Podem Ser Vazias!\n";
                valido = false;
            }

            return valido;
        }
        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            if (e.Sucesso)
            {
                IssoEUmUpdate = true;
                viewModelStrategy = new EditarProdutoVMStrategy();
                OnPropertyChanged("ProdutoGrades");
                ProdutoGradeComposicaoPreco = ProdutoGrades[0];
            }
            //Entidade = new ProdutoModel();
            //Entidade.CodBarra = Entidade.Descricao = string.Empty;
            //Entidade.Fornecedor = Fornecedores[0];
            //Entidade.Marca = Marcas[0];
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

        public ObservableCollection<FornecedorModel> Fornecedores
        {
            get => _fornecedores;
            set
            {
                _fornecedores = value;
                OnPropertyChanged("Fornecedores");
            }
        }
        public ObservableCollection<MarcaModel> Marcas
        {
            get => _marcas;
            set
            {
                _marcas = value;
                OnPropertyChanged("Marcas");
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

        public ProdutoGrade ProdutoGradeSelecionada
        {
            get => _produtoGradeSelecionada;
            set
            {
                _produtoGradeSelecionada = value;
                OnPropertyChanged("ProdutoGradeSelecionada");
            }
        }

        public ObservableCollection<ComposicaoPreco> ComposicaoPrecos
        {
            get => _composicaoPrecos;
            set
            {
                _composicaoPrecos = value;
                OnPropertyChanged("ComposicaoPrecos");
            }
        }

        public ProdutoGrade ProdutoGradeComposicaoPreco
        {
            get => _produtoGradeComposicaoPreco;
            set
            {
                _produtoGradeComposicaoPreco = value;
                OnPropertyChanged("ProdutoGradeComposicaoPreco");
            }
        }

        public double Frete
        {
            get => _frete;
            set
            {
                _frete = value;
                OnPropertyChanged("Frete");
            }
        }

        public double PrecoCompra
        {
            get => _precoCompra;
            set
            {
                _precoCompra = value;
                OnPropertyChanged("PrecoCompra");
            }
        }

        public double MediaCustoTotal
        {
            get => _mediaCustoTotal;
            set
            {
                _mediaCustoTotal = value;
                OnPropertyChanged("MediaCustoTotal");
            }
        }
        public double MediaMargemContribuicao
        {
            get => _mediaMargemContribuicao;
            set
            {
                _mediaMargemContribuicao = value;
                OnPropertyChanged("MediaMargemContribuicao");
            }
        }
        public double MediaLucro
        {
            get => _mediaLucro;
            set
            {
                _mediaLucro = value;
                OnPropertyChanged("MediaLucro");
            }
        }

        public double PrecoVenda
        {
            get => _precoVenda;
            set
            {
                _precoVenda = value;
                OnPropertyChanged("PrecoVenda");
            }
        }

        public bool AplicaIcms
        {
            get => _aplicaIcms;
            set
            {
                _aplicaIcms = value;
                OnPropertyChanged("AplicaIcms");
            }
        }

        public ObservableCollection<HistoricoProdutoGrade> HistoricoProdutoGrade
        {
            get => _historicoProdutoGrade;
            set
            {
                _historicoProdutoGrade = value;
                OnPropertyChanged("HistoricoProdutoGrade");
            }
        }

        public bool AplicaTodasProdutoGrade
        {
            get => _aplicaTodasProdutoGrade;
            set
            {
                _aplicaTodasProdutoGrade = value;
                OnPropertyChanged("AplicaTodasProdutoGrade");
            }
        }

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<FornecedorModel>(await daoFornecedor.Listar());
            Fornecedores.Insert(0, new FornecedorModel(GetResource.GetString("fornecedor_nao_selecionado")));
        }
        private async void GetMarcas()
        {
            Marcas = new ObservableCollection<MarcaModel>(await daoMarca.Listar());
            Marcas.Insert(0, new MarcaModel(GetResource.GetString("marca_nao_selecionada")));
        }
        private async void GetTiposGrade()
        {
            TiposGrade = new ObservableCollection<Model.TipoGrade>(await daoTipoGrade.Listar());
        }
        private async void GetMatrizes()
        {
            matrizes = await daoLoja.ListarMatrizes();
        }
        public async override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CodBarra":
                    var result = await (daoEntidade as DAOProduto).ListarPorCodigoDeBarraUnico(Entidade.CodBarra);

                    if (result != null && !result.Deletado)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    //ProdutoGrade.Produto = Entidade;

                    break;
            }
        }
    }
}
