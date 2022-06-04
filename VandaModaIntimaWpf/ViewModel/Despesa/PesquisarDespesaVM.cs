using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class PesquisarDespesaVM : APesquisarViewModel<Model.Despesa>
    {
        private DateTime _dataEscolhida;
        private string _filtrarPor;
        private DAO<Model.TipoDespesa> daoTipoDespesa;
        private DAO<Model.Loja> daoLoja;
        private ObservableCollection<Model.TipoDespesa> _tiposDespesa;
        private ObservableCollection<Model.Loja> _lojas;
        private Model.Loja _loja;
        private double _totalEmDespesas;
        private double _totalGeralDespesas;
        private int _abaSelecionada = 0;

        private ObservableCollection<EntidadeComCampo<Model.Despesa>> _despesasEmpresarial;
        private ObservableCollection<EntidadeComCampo<Model.Despesa>> _despesasFamiliar;
        private ObservableCollection<EntidadeComCampo<Model.Despesa>> _despesasResidencial;

        public ICommand AbrirDespesaGroupByLojaComando { get; set; }

        public PesquisarDespesaVM()
        {
            daoEntidade = new DAODespesa(_session);
            daoTipoDespesa = new DAO<TipoDespesa>(_session);
            daoLoja = new DAO<Model.Loja>(_session);
            pesquisarViewModelStrategy = new PesquisarDespesaVMStrategy();
            excelStrategy = new DespesaExcelStrategy(_session);

            AbrirDespesaGroupByLojaComando = new RelayCommand(AbrirDespesaGroupByLoja);

            GetTiposDespesa();
            GetLojas();

            PropertyChanged += PesquisarDespesaVM_PropertyChanged;

            FiltrarPor = "Sem Filtro";
            DataEscolhida = DateTime.Now;
            AbaSelecionada = 0;
        }

        private void AbrirDespesaGroupByLoja(object obj)
        {
            _windowService.ShowDialog(new DespesaGroupByDescricaoViewModel(), null);
        }

        private void PesquisarDespesaVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "AbaSelecionada":
                    if (DespesasEmpresarial == null || DespesasFamiliar == null || DespesasResidencial == null)
                        return;

                    switch (AbaSelecionada)
                    {
                        case 0:
                            TotalEmDespesas = DespesasEmpresarial.Sum(s => s.Entidade.Valor);
                            break;
                        case 1:
                            TotalEmDespesas = DespesasFamiliar.Sum(s => s.Entidade.Valor);
                            break;
                        case 2:
                            TotalEmDespesas = DespesasResidencial.Sum(s => s.Entidade.Valor);
                            break;
                    }
                    break;
                case "FiltrarPor":
                    if (FiltrarPor.Equals("Sem Filtro"))
                        TermoPesquisa = string.Empty;
                    break;
            }
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public async override Task PesquisaItens(string termo)
        {
            if (FiltrarPor == null)
                return;

            if (FiltrarPor.Equals("Sem Filtro") && TermoPesquisa?.Length > 0)
                return;

            if (DataEscolhida.Year == 1)
                return;

            DAODespesa dao = (DAODespesa)daoEntidade;

            TotalGeralDespesas = await dao.RetornaSomaTodasDespesas(DataEscolhida);

            var tipoEmpresarial = TiposDespesa.Where(w => w.Nome.Equals("DESPESA EMPRESARIAL")).Single();
            var tipoFamiliar = TiposDespesa.Where(w => w.Nome.Equals("DESPESA FAMILIAR")).Single();
            var tipoResidencial = TiposDespesa.Where(w => w.Nome.Equals("DESPESA RESIDENCIAL")).Single();

            //TODO: USAR FUTURE QUERIES
            DespesasEmpresarial = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoEmpresarial, Loja, DataEscolhida, FiltrarPor, TermoPesquisa)));
            DespesasFamiliar = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoFamiliar, Loja, DataEscolhida, FiltrarPor, TermoPesquisa)));
            DespesasResidencial = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoResidencial, Loja, DataEscolhida, FiltrarPor, TermoPesquisa)));

            OnPropertyChanged("AbaSelecionada");
        }
        private async void GetTiposDespesa()
        {
            TiposDespesa = new ObservableCollection<TipoDespesa>(await daoTipoDespesa.Listar());
            TiposDespesa.Insert(0, new TipoDespesa { Nome = "TODOS OS TIPOS" });
        }
        private async void GetLojas()
        {
            Lojas = new ObservableCollection<Model.Loja>(await daoLoja.Listar());
            Lojas.Insert(0, new Model.Loja("TODAS AS LOJAS"));
            Loja = Lojas[0];
        }

        protected override WorksheetContainer<Model.Despesa>[] GetWorksheetContainers()
        {
            var listas = new WorksheetContainer<Model.Despesa>[3];

            WorksheetContainer<Model.Despesa> containerEmpresarial = new WorksheetContainer<Model.Despesa>
            {
                Nome = "DESPESA EMPRESARIAL",
                Lista = DespesasEmpresarial.Select(s => s.Entidade).ToList()
            };

            WorksheetContainer<Model.Despesa> containerFamiliar = new WorksheetContainer<Model.Despesa>
            {
                Nome = "DESPESA FAMILIAR",
                Lista = DespesasFamiliar.Select(s => s.Entidade).ToList()
            };

            WorksheetContainer<Model.Despesa> containerResidencial = new WorksheetContainer<Model.Despesa>
            {
                Nome = "DESPESA RESIDENCIAL",
                Lista = DespesasResidencial.Select(s => s.Entidade).ToList()
            };

            listas[0] = containerEmpresarial;
            listas[1] = containerFamiliar;
            listas[2] = containerResidencial;

            return listas;
        }

        public override object GetCadastrarViewModel()
        {
            return new CadastrarDespesaVM(_session);
        }

        public override object GetEditarViewModel()
        {
            return new EditarDespesaVM(_session, EntidadeSelecionada.Entidade);
        }

        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
                OnPropertyChanged("AbaSelecionada");
            }
        }
        public string FiltrarPor
        {
            get => _filtrarPor;
            set
            {
                _filtrarPor = value;
                OnPropertyChanged("FiltrarPor");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public ObservableCollection<TipoDespesa> TiposDespesa
        {
            get => _tiposDespesa;
            set
            {
                _tiposDespesa = value;
                OnPropertyChanged("TiposDespesa");
            }
        }

        public double TotalEmDespesas
        {
            get => _totalEmDespesas;
            set
            {
                _totalEmDespesas = value;
                OnPropertyChanged("TotalEmDespesas");
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
        public Model.Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public int AbaSelecionada
        {
            get => _abaSelecionada;
            set
            {
                _abaSelecionada = value;
                OnPropertyChanged("AbaSelecionada");
            }
        }

        public double TotalGeralDespesas
        {
            get => _totalGeralDespesas;
            set
            {
                _totalGeralDespesas = value;
                OnPropertyChanged("TotalGeralDespesas");
            }
        }

        public ObservableCollection<EntidadeComCampo<Model.Despesa>> DespesasEmpresarial
        {
            get => _despesasEmpresarial;
            set
            {
                _despesasEmpresarial = value;
                OnPropertyChanged("DespesasEmpresarial");
            }
        }
        public ObservableCollection<EntidadeComCampo<Model.Despesa>> DespesasFamiliar
        {
            get => _despesasFamiliar;
            set
            {
                _despesasFamiliar = value;
                OnPropertyChanged("DespesasFamiliar");
            }
        }
        public ObservableCollection<EntidadeComCampo<Model.Despesa>> DespesasResidencial
        {
            get => _despesasResidencial;
            set
            {
                _despesasResidencial = value;
                OnPropertyChanged("DespesasResidencial");
            }
        }
    }
}
