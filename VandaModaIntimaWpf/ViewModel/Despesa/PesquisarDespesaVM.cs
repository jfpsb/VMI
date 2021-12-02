using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Arquivo;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

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
        private ObservableCollection<EntidadeComCampo<Model.Despesa>> _outrasDespesas;

        public PesquisarDespesaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.Despesa> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAODespesa(_session);
            daoTipoDespesa = new DAO<TipoDespesa>(_session);
            daoLoja = new DAO<Model.Loja>(_session);
            pesquisarViewModelStrategy = new PesquisarDespesaVMStrategy();
            excelStrategy = new DespesaExcelStrategy(_session);

            GetTiposDespesa();
            GetLojas();

            PropertyChanged += PesquisarDespesaVM_PropertyChanged;

            FiltrarPor = "Sem Filtro";
            DataEscolhida = DateTime.Now;
            AbaSelecionada = 0;
        }

        private void PesquisarDespesaVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AbaSelecionada"))
            {
                if (DespesasEmpresarial == null || DespesasFamiliar == null || DespesasResidencial == null
                    || OutrasDespesas == null)
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
                    case 3:
                        TotalEmDespesas = OutrasDespesas.Sum(s => s.Entidade.Valor);
                        break;
                }
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

            if (DataEscolhida.Year == 1)
                return;

            DAODespesa dao = (DAODespesa)daoEntidade;

            TotalGeralDespesas = await dao.RetornaSomaTodasDespesas(DataEscolhida);

            var tipoEmpresarial = TiposDespesa.Where(w => w.Nome.Equals("DESPESA EMPRESARIAL")).Single();
            var tipoFamiliar = TiposDespesa.Where(w => w.Nome.Equals("DESPESA FAMILIAR")).Single();
            var tipoResidencial = TiposDespesa.Where(w => w.Nome.Equals("DESPESA RESIDENCIAL")).Single();
            var tipoOutras = TiposDespesa.Where(w => w.Nome.Equals("OUTRAS DESPESAS")).Single();

            DespesasEmpresarial = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoEmpresarial, Loja, DataEscolhida, FiltrarPor, TermoPesquisa)));
            DespesasFamiliar = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoFamiliar, Loja, DataEscolhida, FiltrarPor, TermoPesquisa)));
            DespesasResidencial = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoResidencial, Loja, DataEscolhida, FiltrarPor, TermoPesquisa)));
            OutrasDespesas = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoOutras, Loja, DataEscolhida, FiltrarPor, TermoPesquisa)));
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
            var listas = new WorksheetContainer<Model.Despesa>[4];

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

            WorksheetContainer<Model.Despesa> containerOutras = new WorksheetContainer<Model.Despesa>
            {
                Nome = "OUTRAS DESPESAS",
                Lista = OutrasDespesas.Select(s => s.Entidade).ToList()
            };

            listas[0] = containerEmpresarial;
            listas[1] = containerFamiliar;
            listas[2] = containerResidencial;
            listas[3] = containerOutras;

            return listas;
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
                OnPropertyChanged("AbaSelecionada");
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
        public ObservableCollection<EntidadeComCampo<Model.Despesa>> OutrasDespesas
        {
            get => _outrasDespesas;
            set
            {
                _outrasDespesas = value;
                OnPropertyChanged("OutrasDespesas");
            }
        }
    }
}
