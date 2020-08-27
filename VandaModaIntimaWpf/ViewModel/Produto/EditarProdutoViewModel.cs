using Newtonsoft.Json;
using NHibernate;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.Model;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoViewModel : CadastrarProdutoViewModel
    {
        public EditarProdutoViewModel(ISession session, ProdutoModel produto) : base(session)
        {
            Entidade = produto;
            CodigosFornecedor = new ObservableCollection<string>(Entidade.Codigos);
            GetUltimoLog();
        }
        protected async override Task<AposCriarDocumentoEventArgs> ExecutarSalvar()
        {
            CouchDbResponse couchDbResponse;
            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs();

            if (ultimoLog != null)
            {
                e.CouchDbLog = (CouchDbProdutoLog)ultimoLog.Clone();
                ultimoLog.AtribuiCampos(Entidade);
                couchDbResponse = await couchDbClient.UpdateDocument(ultimoLog);
                e.CouchDbLog.Rev = couchDbResponse.Rev;
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(Entidade);
                couchDbResponse = await couchDbClient.CreateDocument(Entidade.CouchDbId(), jsonData);
            }

            e.CouchDbResponse = couchDbResponse;
            e.MensagemSucesso = cadastrarViewModelStrategy.MensagemDocumentoAtualizadoSucesso();
            e.MensagemErro = cadastrarViewModelStrategy.MensagemDocumentoNaoAtualizado();
            e.ObjetoSalvo = Entidade;

            return e;
        }

        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            await AtualizarNoBancoDeDados(e);
        }

        private async void GetUltimoLog()
        {
            ultimoLog = (CouchDbProdutoLog)await couchDbClient.FindById(Entidade.CodBarra);
        }

        public new ProdutoModel Entidade
        {
            get { return Entidade; }
            set
            {
                Entidade = value;
                FornecedorComboBox = value.Fornecedor;
                MarcaComboBox = value.Marca;
                OnPropertyChanged("Entidade");
            }
        }

        public FornecedorModel FornecedorComboBox
        {
            get
            {
                if (Entidade.Fornecedor == null)
                {
                    Entidade.Fornecedor = Fornecedores[0];
                }

                return Entidade.Fornecedor;
            }

            set
            {
                Entidade.Fornecedor = value;
                OnPropertyChanged("FornecedorComboBox");
            }
        }

        public MarcaModel MarcaComboBox
        {
            get
            {
                if (Entidade.Marca == null)
                {
                    Entidade.Marca = Marcas[0];
                }

                return Entidade.Marca;
            }

            set
            {
                Entidade.Marca = value;
                OnPropertyChanged("MarcaComboBox");
            }
        }
    }
}
