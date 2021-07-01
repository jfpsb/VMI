using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class PesquisarDespesaVM : APesquisarViewModel<Model.Despesa>
    {
        private string _tipoDespesaNome;
        private DateTime _dataEscolhida;
        private string _filtrarPor;
        private DAO<Model.TipoDespesa> daoTipoDespesa;
        private ObservableCollection<Model.TipoDespesa> _tiposDespesa;
        private double _totalEmDespesas;

        public PesquisarDespesaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.Despesa> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAODespesa(_session);
            daoTipoDespesa = new DAO<TipoDespesa>(_session);
            pesquisarViewModelStrategy = new PesquisarDespesaVMStrategy();

            GetTiposDespesa();

            TipoDespesaNome = "TODOS OS TIPOS";
            FiltrarPor = "Sem Filtro";
            DataEscolhida = DateTime.Now;
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public async override Task PesquisaItens(string termo)
        {
            if (TipoDespesaNome == null)
                return;

            if (FiltrarPor == null)
                return;

            if (DataEscolhida.Year == 1)
                return;

            DAODespesa dao = (DAODespesa)daoEntidade;
            TipoDespesa tipoDespesa = TiposDespesa.Where(w => w.Nome.Equals(TipoDespesaNome)).Single();
            Entidades = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoDespesa, DataEscolhida, FiltrarPor, TermoPesquisa)));

            TotalEmDespesas = Entidades.Select(s => s.Entidade).Sum(sum => sum.Valor);
        }
        private async Task GetTiposDespesa()
        {
            TiposDespesa = new ObservableCollection<TipoDespesa>(await daoTipoDespesa.Listar());
            TiposDespesa.Insert(0, new TipoDespesa { Nome = "TODOS OS TIPOS" });
        }
        public string TipoDespesaNome
        {
            get => _tipoDespesaNome;
            set
            {
                _tipoDespesaNome = value;
                OnPropertyChanged("TipoDespesaNome");
                OnPropertyChanged("TermoPesquisa");
            }
        }
        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
                OnPropertyChanged("TermoPesquisa");
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
    }
}
