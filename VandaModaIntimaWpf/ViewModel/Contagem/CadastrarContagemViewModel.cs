using NHibernate;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View;
using ContagemModel = VandaModaIntimaWpf.Model.Contagem;
using LojaModel = VandaModaIntimaWpf.Model.Loja;
using TipoContagemModel = VandaModaIntimaWpf.Model.TipoContagem;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    class CadastrarContagemViewModel : ACadastrarViewModel
    {
        protected DAOContagem _daoContagem;
        private DAOTipoContagem _daoTipoContagem;
        private DAOLoja _daoLoja;
        private ContagemModel _contagem;

        public ObservableCollection<LojaModel> Lojas { get; set; }
        public ObservableCollection<TipoContagemModel> TiposContagem { get; set; }

        public CadastrarContagemViewModel(ISession session)
        {
            _session = session;

            Contagem = new ContagemModel();

            _daoLoja = new DAOLoja(_session);
            _daoTipoContagem = new DAOTipoContagem(_session);
            _daoContagem = new DAOContagem(_session);

            GetLojas();
            GetTiposContagem();
        }
        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ResetaPropriedades()
        {
            Contagem = new ContagemModel();
            Contagem.TipoContagem = TiposContagem[0];
            Contagem.Loja = Lojas[0];
        }

        public override async void Salvar(object parameter)
        {
            Contagem.Data = DateTime.Now;
            Contagem.Finalizada = false;

            _result = await _daoContagem.Inserir(Contagem);

            if (_result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso("Contagem Cadastrada Com Sucesso");
                return;
            }

            SetStatusBarErro("Erro ao Cadastrar Contagem");
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

        public ContagemModel Contagem
        {
            get
            {
                return _contagem;
            }

            set
            {
                _contagem = value;
                OnPropertyChanged("Contagem");
            }
        }
    }
}
