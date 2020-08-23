using Newtonsoft.Json;
using NHibernate;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorManualmenteViewModel : ACadastrarViewModel
    {
        protected DAOFornecedor daoFornecedor;
        protected FornecedorModel fornecedor;
        public CadastrarFornecedorManualmenteViewModel(ISession session)
        {
            _session = session;
            daoFornecedor = new DAOFornecedor(_session);
            fornecedor = new FornecedorModel();
            fornecedor.PropertyChanged += CadastrarViewModel_PropertyChanged;
        }
        public override async void Salvar(object parameter)
        {
            string fornecedorJson = JsonConvert.SerializeObject(Fornecedor);
            var couchDbResponse = await couchDbClient.CreateDocument(Fornecedor.Cnpj, fornecedorJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "Fornecedor Cadastrado Com Sucesso",
                MensagemErro = "Erro Ao Cadastrar Fornecedor",
                ObjetoSalvo = Fornecedor
            };

            ChamaAposCriarDocumento(e);
        }

        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cnpj":
                    var result = await daoFornecedor.ListarPorId(Fornecedor.Cnpj);

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
        public override void ResetaPropriedades()
        {
            Fornecedor = new FornecedorModel();
            Fornecedor.Cnpj = Fornecedor.Nome = Fornecedor.Fantasia = Fornecedor.Email = string.Empty;
        }
        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Fornecedor.Cnpj)
                || string.IsNullOrEmpty(Fornecedor.Nome)
                || Fornecedor.Cnpj.Length != 14)
            {
                return false;
            }

            return true;
        }

        public override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public FornecedorModel Fornecedor
        {
            get { return fornecedor; }
            set
            {
                fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }
    }
}
