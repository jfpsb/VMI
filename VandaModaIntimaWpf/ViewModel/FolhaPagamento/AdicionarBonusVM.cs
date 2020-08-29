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
            cadastrarViewModelStrategy = new CadastrarBonusMsgVMStrategy();
            _folha = folha;

            Entidade = new Bonus() { Id = DateTime.Now.Ticks };
            Entidade.Folha = folha;
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }
        public override void ResetaPropriedades()
        {
            Entidade = new Bonus();
            Entidade.Folha = _folha;
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Entidade.Descricao) || Entidade.Valor <= 0.0)
                return false;

            return true;
        }

        protected override void ExecutarAntesCriarDocumento()
        {
            Entidade.Data = DateTime.Now;
        }
    }
}
