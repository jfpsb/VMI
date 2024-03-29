﻿using NHibernate;
using System;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarBonusVM : ACadastrarViewModel<Bonus>
    {
        private Model.FolhaPagamento _folha;
        private DateTime _inicioPagamento;
        private DateTime dataEscolhida;
        private DAOBonusMensal daoBonusMensal;

        public AdicionarBonusVM(ISession session, Model.FolhaPagamento folha, DateTime dataEscolhida) : base(session, false)
        {
            daoEntidade = new DAOBonus(_session);
            daoBonusMensal = new DAOBonusMensal(_session);
            viewModelStrategy = new AdicionarBonusVMStrategy();
            Folha = folha;
            this.dataEscolhida = dataEscolhida;

            AntesDeInserirNoBancoDeDados += ConfiguraBonus;

            Entidade = new Bonus()
            {
                Funcionario = folha.Funcionario,
                LojaTrabalho = folha.Funcionario.LojaTrabalho
            };

            PropertyChanged += AdicionarBonusVM_PropertyChanged;

            InicioPagamento = new DateTime(dataEscolhida.Year, dataEscolhida.Month, 1);

            AposInserirNoBancoDeDados += SalvarBonusMensal;
        }

        private async void SalvarBonusMensal(AposInserirBDEventArgs e)
        {
            var bonusInserido = await daoEntidade.ListarPorUuid((Guid)e.UuidEntidade);

            if (e.Sucesso && bonusInserido.BonusMensal)
            {
                BonusMensal bonusMensal = new BonusMensal
                {
                    Descricao = bonusInserido.Descricao,
                    Valor = bonusInserido.Valor,
                    Funcionario = bonusInserido.Funcionario,
                    PagoEmFolha = bonusInserido.PagoEmFolha
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

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            Entidade = new Bonus
            {
                Funcionario = Folha.Funcionario,
                LojaTrabalho = Folha.Funcionario.LojaTrabalho
            };

            Entidade.Descricao = string.Empty;
            Entidade.Valor = 0.0;

            InicioPagamento = new DateTime(dataEscolhida.Year, dataEscolhida.Month, 1);
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            BtnSalvarToolTip = "";
            bool valido = true;

            if (string.IsNullOrEmpty(Entidade.Descricao))
            {
                BtnSalvarToolTip += "O CAMPO DE DESCRIÇÃO NÃO PODE SER VAZIO\n";
                valido = false;
            }

            if (Entidade.Valor <= 0.0)
            {
                BtnSalvarToolTip += "O VALOR DO BÔNUS NÃO PODE SER MENOR OU IGUAL A ZERO\n";
                valido = false;
            }

            return valido;
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

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

        public Model.FolhaPagamento Folha
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
