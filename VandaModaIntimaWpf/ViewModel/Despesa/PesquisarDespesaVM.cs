﻿using System;
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
        private DAO<Model.Loja> daoLoja;
        private ObservableCollection<Model.TipoDespesa> _tiposDespesa;
        private ObservableCollection<Model.Loja> _lojas;
        private Model.Loja _loja;
        private double _totalEmDespesas;

        public PesquisarDespesaVM(IMessageBoxService messageBoxService, IAbrePelaTelaPesquisaService<Model.Despesa> abrePelaTelaPesquisaService) : base(messageBoxService, abrePelaTelaPesquisaService)
        {
            daoEntidade = new DAODespesa(_session);
            daoTipoDespesa = new DAO<TipoDespesa>(_session);
            daoLoja = new DAO<Model.Loja>(_session);
            pesquisarViewModelStrategy = new PesquisarDespesaVMStrategy();

            GetTiposDespesa();
            GetLojas();

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
            Entidades = new ObservableCollection<EntidadeComCampo<Model.Despesa>>(EntidadeComCampo<Model.Despesa>.CriarListaEntidadeComCampo(await dao.ListarPorTipoDespesaFiltroMesAno(tipoDespesa, Loja, DataEscolhida, FiltrarPor, TermoPesquisa)));

            TotalEmDespesas = Entidades.Select(s => s.Entidade).Sum(sum => sum.Valor);
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
    }
}
