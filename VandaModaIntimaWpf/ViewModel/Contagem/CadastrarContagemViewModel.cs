using Newtonsoft.Json;
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
    class CadastrarContagemViewModel : ACadastrarViewModel
    {
        protected DAOContagem daoContagem;
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
            daoContagem = new DAOContagem(_session);

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

            string contagemJson = JsonConvert.SerializeObject(Contagem);
            var couchDbResponse = await couchDbClient.CreateDocument(Contagem.ToString(), contagemJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "Contagem Cadastrada Com Sucesso",
                MensagemErro = "Erro ao Cadastrar Contagem",
                ObjetoSalvo = Contagem
            };

            ChamaAposCriarDocumento(e);
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

        public override async void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoContagem.Inserir(Contagem);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    MensagemSucesso = "Contagem Inserida com Sucesso",
                    MensagemErro = "Erro ao Inserir Contagem",
                    ObjetoSalvo = Contagem,
                    CouchDbResponse = e.CouchDbResponse
                };

                ChamaAposInserirNoBD(e2);
            }
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
