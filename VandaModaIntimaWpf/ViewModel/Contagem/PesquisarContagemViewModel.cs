using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class PesquisarContagemViewModel : APesquisarViewModel<ContagemModel>
    {
        private DAOLoja daoLoja;
        public ObservableCollection<LojaModel> Lojas { get; set; }

        private DateTime _dataInicial;
        private DateTime _dataFinal;
        private LojaModel _loja;

        public PesquisarContagemViewModel() : base("Contagem")
        {
            daoLoja = new DAOLoja(_session);
            daoEntidade = new DAOContagem(_session);
            pesquisarViewModelStrategy = new PesquisarContagemViewModelStrategy();
            DataInicial = DateTime.Now;
            DataFinal = DateTime.Now;
            GetLojas();
        }
        public override async void GetItems(string termo)
        {
            DAOContagem daoContagem = (DAOContagem)daoEntidade;
            Entidades = new ObservableCollection<EntidadeComCampo<ContagemModel>>(EntidadeComCampo<ContagemModel>.ConverterIList(await daoContagem.ListarPorLojaEPeriodo(Loja, DataInicial, DataFinal)));
        }

        public override bool IsEditable(object parameter)
        {
            return true;
        }

        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await daoLoja.Listar<LojaModel>());
        }

        public DateTime DataInicial
        {
            get
            {
                return _dataInicial;
            }

            set
            {
                _dataInicial = value;
                OnPropertyChanged("DataInicial");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public DateTime DataFinal
        {
            get
            {
                return _dataFinal;
            }

            set
            {
                _dataFinal = value.AddDays(1).AddSeconds(-1);
                OnPropertyChanged("DataFinal");
                OnPropertyChanged("TermoPesquisa");
            }
        }

        public LojaModel Loja
        {
            get
            {
                return _loja;
            }

            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
                OnPropertyChanged("TermoPesquisa");
            }
        }
    }
}
