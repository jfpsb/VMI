using NHibernate;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
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
        private DateTime dataEscolhida;
        private DAOBonusMensal daoBonusMensal;

        public AdicionarBonusVM(ISession session, FolhaModel folha, DateTime dataEscolhida, IMessageBoxService messageBoxService, bool issoEUmUpdate) : base(session, messageBoxService, issoEUmUpdate)
        {
            daoEntidade = new DAOBonus(session);
            daoBonusMensal = new DAOBonusMensal(session);
            viewModelStrategy = new CadastrarBonusVMStrategy();
            Folha = folha;
            this.dataEscolhida = dataEscolhida;

            AntesDeInserirNoBancoDeDados += ConfiguraBonus;

            Entidade = new Bonus()
            {
                Funcionario = folha.Funcionario
            };

            PropertyChanged += AdicionarBonusVM_PropertyChanged;

            InicioPagamento = new DateTime(dataEscolhida.Year, dataEscolhida.Month, 1);

            AposInserirNoBancoDeDados += SalvarBonusMensal;
        }

        private async void SalvarBonusMensal(AposInserirBDEventArgs e)
        {
            var bonusInserido = await daoEntidade.ListarPorId(e.IdentificadorEntidade) as Bonus;

            if (e.Sucesso && bonusInserido.BonusMensal)
            {
                BonusMensal bonusMensal = new BonusMensal
                {
                    Descricao = bonusInserido.Descricao,
                    Valor = bonusInserido.Valor,
                    Funcionario = bonusInserido.Funcionario
                };

                await daoBonusMensal.Inserir(bonusMensal);
            }
        }

        private void AdicionarBonusVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

            Entidade.Descricao = string.Empty;
            Entidade.Valor = 0.0;

            InicioPagamento = new DateTime(dataEscolhida.Year, dataEscolhida.Month, 1);
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            double valor;

            if (string.IsNullOrEmpty(Entidade.Descricao) || !double.TryParse(Valor, out valor))
                return false;

            Entidade.Valor = valor;

            return true;
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

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
