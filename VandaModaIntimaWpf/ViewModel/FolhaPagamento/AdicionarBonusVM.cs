using NHibernate;
using System;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarBonusVM : ACadastrarViewModel<Bonus>
    {
        private FolhaModel _folha;

        public AdicionarBonusVM(ISession session, FolhaModel folha, IMessageBoxService messageBoxService) : base(session, messageBoxService)
        {
            daoEntidade = new DAOBonus(session);
            viewModelStrategy = new CadastrarBonusVMStrategy();
            _folha = folha;

            AntesDeInserirNoBancoDeDados += ConfiguraBonus;

            Entidade = new Bonus() { Id = DateTime.Now.Ticks };
            Entidade.Folha = folha;
        }

        private void ConfiguraBonus()
        {
            Entidade.Data = DateTime.Now;
        }

        public override void ResetaPropriedades()
        {
            Entidade = new Bonus
            {
                Folha = _folha
            };
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Descricao) || Entidade.Valor <= 0.0)
                return false;

            return true;
        }
    }
}
