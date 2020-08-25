using Newtonsoft.Json;
using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using LojaModel = VandaModaIntimaWpf.Model.Loja;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class CadastrarLojaViewModel : ACadastrarViewModel
    {
        protected DAOLoja daoLoja;
        protected LojaModel lojaModel;
        public ObservableCollection<LojaModel> Matrizes { get; set; }
        public CadastrarLojaViewModel(ISession session)
        {
            _session = session;
            daoLoja = new DAOLoja(_session);
            lojaModel = new LojaModel();
            lojaModel.PropertyChanged += CadastrarViewModel_PropertyChanged;
            GetMatrizes();
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Loja.Cnpj) || string.IsNullOrEmpty(Loja.Nome) || Loja.Aluguel <= 0.0)
            {
                return false;
            }

            return true;
        }

        //TODO: colocar strings em resources
        public override async void Salvar(object parameter)
        {
            if (Loja.Matriz.Cnpj == null)
                Loja.Matriz = null;

            string lojaJson = JsonConvert.SerializeObject(Loja);
            var couchDbResponse = await couchDbClient.CreateDocument(Loja.Cnpj, lojaJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "LOG de Inserção de Loja Criado com Sucesso",
                MensagemErro = "Erro ao Criar Log de Inserção de Loja",
                ObjetoSalvo = Loja
            };

            ChamaAposCriarDocumento(e);
        }

        public override void ResetaPropriedades()
        {
            Loja = new LojaModel();
            Loja.Cnpj = Loja.Nome = Loja.Telefone = Loja.Endereco = Loja.InscricaoEstadual = string.Empty;
            Loja.Matriz = Matrizes[0];
        }

        public LojaModel Loja
        {
            get { return lojaModel; }
            set
            {
                lojaModel = value;
                OnPropertyChanged("Loja");
            }
        }
        private async void GetMatrizes()
        {
            Matrizes = new ObservableCollection<LojaModel>(await daoLoja.ListarMatrizes());
            Matrizes.Insert(0, new LojaModel(GetResource.GetString("matriz_nao_selecionada")));
        }

        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoLoja.ListarPorId(Loja.Cnpj);

                    if (result != null)
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoItemJaExiste = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }

        public override async void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoLoja.Inserir(Loja);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    MensagemSucesso = "Loja Inserida com Sucesso",
                    MensagemErro = "Erro ao Inserir Loja",
                    ObjetoSalvo = Loja,
                    CouchDbResponse = e.CouchDbResponse
                };

                ChamaAposInserirNoBD(e2);
            }
        }
    }
}
