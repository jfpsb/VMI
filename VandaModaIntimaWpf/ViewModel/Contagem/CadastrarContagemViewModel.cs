using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;
using LojaModel = VandaModaIntimaWpf.Model.Loja;
using TipoContagemModel = VandaModaIntimaWpf.Model.TipoContagem;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class CadastrarContagemViewModel : ACadastrarViewModel<ContagemModel>
    {
        private DAOTipoContagem _daoTipoContagem;
        private DAOLoja _daoLoja;

        public ObservableCollection<LojaModel> Lojas { get; set; }
        public ObservableCollection<TipoContagemModel> TiposContagem { get; set; }

        public CadastrarContagemViewModel(ISession session) : base(session)
        {
            Entidade = new ContagemModel();
            cadastrarViewModelStrategy = new CadastrarContagemViewModelStrategy();

            _daoLoja = new DAOLoja(_session);
            _daoTipoContagem = new DAOTipoContagem(_session);
            daoEntidade = new DAOContagem(_session);

            GetLojas();
            GetTiposContagem();
        }
        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ResetaPropriedades()
        {
            Entidade = new ContagemModel();
            Entidade.TipoContagem = TiposContagem[0];
            Entidade.Loja = Lojas[0];
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            return true;
        }
        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await _daoLoja.Listar<LojaModel>());
        }
        private async void GetTiposContagem()
        {
            TiposContagem = new ObservableCollection<TipoContagemModel>(await _daoTipoContagem.Listar<TipoContagemModel>());
        }

        protected override void ExecutarAntesCriarDocumento()
        {
            Entidade.Data = DateTime.Now;
            Entidade.Finalizada = false;
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
