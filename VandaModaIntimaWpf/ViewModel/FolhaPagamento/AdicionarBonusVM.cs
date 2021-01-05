using NHibernate;
using System;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarBonusVM : ACadastrarViewModel<Bonus>
    {
        private FolhaModel _folha;
        private string _valor;
        private DateTime _inicioPagamento;

        public AdicionarBonusVM(ISession session, FolhaModel folha, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAOBonus(session);
            viewModelStrategy = new CadastrarBonusVMStrategy();
            Folha = folha;

            AntesDeInserirNoBancoDeDados += ConfiguraBonus;

            Entidade = new Bonus()
            {
                Id = DateTime.Now.Ticks,
                Funcionario = folha.Funcionario
            };

            PropertyChanged += AdicionarBonusVM_PropertyChanged;

            InicioPagamento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        private void AdicionarBonusVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "InicioPagamento":
                    Entidade.MesReferencia = InicioPagamento.Month;
                    Entidade.AnoReferencia = InicioPagamento.Year;
                    break;
            }
        }

        private void ConfiguraBonus()
        {
            Entidade.Data = DateTime.Now;
        }

        public override void ResetaPropriedades()
        {
            Entidade = new Bonus
            {
                Funcionario = Folha.Funcionario
            };
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            double valor;

            if (string.IsNullOrEmpty(Entidade.Descricao) || !double.TryParse(Valor, out valor))
                return false;

            Entidade.Valor = valor;

            return true;
        }

        public string Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }
        public DateTime InicioPagamento
        {
            get => _inicioPagamento;
            set
            {
                _inicioPagamento = value;
                OnPropertyChanged("InicioPagamento");
            }
        }

        public FolhaModel Folha
        {
            get => _folha;
            set
            {
                _folha = value;
                OnPropertyChanged("Folha");
            }
        }
    }
}
