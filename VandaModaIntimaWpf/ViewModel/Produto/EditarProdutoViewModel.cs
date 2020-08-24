using Newtonsoft.Json;
using NHibernate;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.Model;
using VandaModaIntimaWpf.Resources;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoViewModel : CadastrarProdutoViewModel
    {
        private CouchDbProdutoLog ultimoLog;
        public EditarProdutoViewModel(ISession session, ProdutoModel produto) : base(session)
        {
            Produto = produto;
            CodigosFornecedor = new ObservableCollection<string>(Produto.Codigos);
            GetUltimoLog();
        }

        public override async void Salvar(object parameter)
        {
            if (Produto.Fornecedor?.Cnpj == null)
                Produto.Fornecedor = null;

            if (Produto.Marca != null && Produto.Marca.Nome.Equals(GetResource.GetString("marca_nao_selecionada")))
                Produto.Marca = null;

            Produto.Codigos = CodigosFornecedor;

            CouchDbResponse couchDbResponse;

            if (ultimoLog != null)
            {
                ultimoLog.AtribuiCampos(Produto);
                couchDbResponse = await couchDbClient.UpdateDocument(ultimoLog);
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject(Produto);
                couchDbResponse = await couchDbClient.CreateDocument(Produto.CodBarra, jsonData);
            }

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "LOG de Atualização de Produto Criado com Sucesso",
                MensagemErro = "Erro ao Criar Log de Atualização de Produto",
                ObjetoSalvo = Produto
            };

            ChamaAposCriarDocumento(e);
        }

        public async override void InserirNoBancoDeDados(AposCriarDocumentoEventArgs e)
        {
            if (e.CouchDbResponse.Ok)
            {
                _result = await daoProduto.Merge(Produto);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    IssoEUmUpdate = true,
                    MensagemSucesso = "Produto Atualizado com Sucesso",
                    MensagemErro = "Erro ao Atualizar Produto",
                    ObjetoSalvo = Produto
                };

                ChamaAposInserirNoBD(e2);
            }
            else
            {
                //TODO: Reverter update
                //CouchDbResponse couchDbResponse = await couchDbClient.CreateOrUpdateDocument(Produto.CodBarra, );
                //Console.WriteLine(string.Format("DELETANDO {0}: {1}", couchDbResponse.Id, couchDbResponse.Ok));
            }
        }

        private async void GetUltimoLog()
        {
            ultimoLog = (CouchDbProdutoLog)await couchDbClient.FindById(Produto.CodBarra);
        }

        public new ProdutoModel Produto
        {
            get { return produtoModel; }
            set
            {
                produtoModel = value;
                FornecedorComboBox = value.Fornecedor;
                MarcaComboBox = value.Marca;
                OnPropertyChanged("Produto");
            }
        }

        public FornecedorModel FornecedorComboBox
        {
            get
            {
                if (Produto.Fornecedor == null)
                {
                    Produto.Fornecedor = Fornecedores[0];
                }

                return Produto.Fornecedor;
            }

            set
            {
                Produto.Fornecedor = value;
                OnPropertyChanged("FornecedorComboBox");
            }
        }

        public MarcaModel MarcaComboBox
        {
            get
            {
                if (Produto.Marca == null)
                {
                    Produto.Marca = Marcas[0];
                }

                return Produto.Marca;
            }

            set
            {
                Produto.Marca = value;
                OnPropertyChanged("MarcaComboBox");
            }
        }
    }
}
