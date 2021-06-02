using System;
using System.Collections.ObjectModel;
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

        public PesquisarDespesaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.Despesa> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new Model.DAO.DAO<Model.Despesa>(_session);
            daoTipoDespesa = new DAO<TipoDespesa>(_session);
            pesquisarViewModelStrategy = new PesquisarDespesaVMStrategy();

            GetTiposDespesa();
            DataEscolhida = DateTime.Now;
        }

        public override bool Editavel(object parameter)
        {
            return true;
        }

        public override void PesquisaItens(string termo)
        {
            throw new NotImplementedException();
        }
        private async void GetTiposDespesa()
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
            }
        }
        public DateTime DataEscolhida
        {
            get => _dataEscolhida;
            set
            {
                _dataEscolhida = value;
                OnPropertyChanged("DataEscolhida");
            }
        }
        public string FiltrarPor
        {
            get => _filtrarPor;
            set
            {
                _filtrarPor = value;
                OnPropertyChanged("FiltrarPor");
            }
        }

        public ObservableCollection<TipoDespesa> TiposDespesa { get => _tiposDespesa; set => _tiposDespesa = value; }
    }
}
