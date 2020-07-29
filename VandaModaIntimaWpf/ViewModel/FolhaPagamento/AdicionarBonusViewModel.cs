using NHibernate;
using System;
using System.ComponentModel;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarBonusViewModel : ACadastrarViewModel
    {
        private Bonus _bonus;
        private DAOBonus daoBonus;
        private FolhaModel _folha;

        public AdicionarBonusViewModel(ISession session, FolhaModel folha)
        {
            _session = session;
            daoBonus = new DAOBonus(session);
            _folha = folha;

            Bonus = new Bonus() { Id = DateTime.Now.Ticks };
            Bonus.Folha = folha;
        }

        public Bonus Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                OnPropertyChanged("Bonus");
            }
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public override void ResetaPropriedades()
        {
            Bonus = new Bonus();
            Bonus.Folha = _folha;
        }

        public async override void Salvar(object parameter)
        {
            Bonus.Data = DateTime.Now;

            _result = await daoBonus.Inserir(Bonus);

            if (_result)
            {
                _session.Refresh(_folha);
                ResetaPropriedades();
                await SetStatusBarSucesso("Bônus Adicionado Com Sucesso");
                return;
            }

            SetStatusBarErro("Erro ao Adicionar Bônus");
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Bonus.Descricao) || Bonus.Valor <= 0.0)
                return false;

            return true;
        }
    }
}
