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
    class CadastrarContagemVM : ACadastrarViewModel<ContagemModel>
    {
        private DAOTipoContagem _daoTipoContagem;
        private DAOLoja _daoLoja;

        public ObservableCollection<LojaModel> Lojas { get; set; }
        public ObservableCollection<TipoContagemModel> TiposContagem { get; set; }

        public CadastrarContagemVM() : base()
        {
            Entidade = new ContagemModel();
            viewModelStrategy = new CadastrarContagemVMStrategy();

            _daoLoja = new DAOLoja(_session);
            _daoTipoContagem = new DAOTipoContagem(_session);
            daoEntidade = new DAOContagem(_session);

            AntesDeInserirNoBancoDeDados += ConfiguraContagem;

            GetLojas();
            GetTiposContagem();
        }

        private void ConfiguraContagem()
        {
            Entidade.Data = DateTime.Now;
            Entidade.Finalizada = false;
        }

        public override void ResetaPropriedades(AposInserirBDEventArgs e)
        {
            Entidade = new ContagemModel
            {
                TipoContagem = TiposContagem[0],
                Loja = Lojas[0]
            };
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            return true;
        }
        private async void GetLojas()
        {
            Lojas = new ObservableCollection<LojaModel>(await _daoLoja.Listar());
        }
        private async void GetTiposContagem()
        {
            TiposContagem = new ObservableCollection<TipoContagemModel>(await _daoTipoContagem.Listar());
        }

        public override void Entidade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
    }
}
