using Newtonsoft.Json;
using NHibernate;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Resources;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class CadastrarProdutoViewModel : ACadastrarViewModel
    {
        protected DAOProduto daoProduto;
        protected DAOMarca daoMarca;
        protected DAOFornecedor daoFornecedor;
        protected ProdutoModel produtoModel;

        private string _codigoFornecedor;

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }
        public ObservableCollection<string> CodigosFornecedor { get; set; }
        public ICommand InserirCodigoComando { get; set; }
        public CadastrarProdutoViewModel(ISession session)
        {
            _session = session;
            daoProduto = new DAOProduto(_session);
            daoMarca = new DAOMarca(_session);
            daoFornecedor = new DAOFornecedor(_session);
            Produto = new ProdutoModel();

            Produto.PropertyChanged += CadastrarViewModel_PropertyChanged;
            InserirCodigoComando = new RelayCommand(InserirCodigo, ValidaCodigoFornecedor);

            GetFornecedores();
            GetMarcas();
            CodigosFornecedor = new ObservableCollection<string>();

            AtribuiNovoCodBarra();
        }

        private bool ValidaCodigoFornecedor(object arg)
        {
            return CodigoFornecedor != null && !(CodigoFornecedor.Length == 0);
        }

        private void InserirCodigo(object obj)
        {
            if (!CodigosFornecedor.Contains(CodigoFornecedor))
            {
                CodigosFornecedor.Add(CodigoFornecedor);
                CodigoFornecedor = string.Empty;
                SetStatusBarAguardando();
            }
            else
            {
                SetStatusBarErro("Código Já Está Inserido");
            }
        }

        public override bool ValidacaoSalvar(object parameter)
        {
            if (string.IsNullOrEmpty(Produto.CodBarra) || string.IsNullOrEmpty(Produto.Descricao))
            {
                return false;
            }

            if (Produto.Preco.ToString().Equals(string.Empty) || Produto.Preco == 0)
            {
                return false;
            }

            if (!IsEnabled)
                return false;

            return true;
        }

        public override async void Salvar(object parameter)
        {
            if (Produto.Fornecedor?.Cnpj == null)
                Produto.Fornecedor = null;

            if (Produto.Marca != null && Produto.Marca.Nome.Equals(GetResource.GetString("marca_nao_selecionada")))
                Produto.Marca = null;

            Produto.Codigos = CodigosFornecedor;

            string produtoJson = JsonConvert.SerializeObject(Produto);
            var couchDbResponse = await couchDbClient.CreateDocument(Produto.CodBarra, produtoJson);

            AposCriarDocumentoEventArgs e = new AposCriarDocumentoEventArgs()
            {
                CouchDbResponse = couchDbResponse,
                MensagemSucesso = "LOG de Inserção de Produto Criado com Sucesso",
                MensagemErro = "Erro ao Criar Log de Inserção de Produto",
                ObjetoSalvo = Produto
            };

            ChamaAposCriarDocumento(e);
        }

        public override void ResetaPropriedades()
        {
            Produto = new ProdutoModel();
            Produto.CodBarra = Produto.Descricao = string.Empty;
            Produto.Preco = 0;
            Produto.Fornecedor = Fornecedores[0];
            Produto.Marca = Marcas[0];

            AtribuiNovoCodBarra();
        }
        private void AtribuiNovoCodBarra()
        {
            int novoCodBarra = daoProduto.GetMaxId();
            Produto.CodBarra = (novoCodBarra + 1).ToString();
        }
        public ProdutoModel Produto
        {
            get { return produtoModel; }
            set
            {
                produtoModel = value;
                OnPropertyChanged("Produto");
            }
        }

        public string CodigoFornecedor
        {
            get
            {
                return _codigoFornecedor;
            }

            set
            {
                _codigoFornecedor = value;
                OnPropertyChanged("CodigoFornecedor");
            }
        }

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<FornecedorModel>(await daoFornecedor.Listar<FornecedorModel>());
            Fornecedores.Insert(0, new FornecedorModel(GetResource.GetString("fornecedor_nao_selecionado")));
        }

        private async void GetMarcas()
        {
            Marcas = new ObservableCollection<MarcaModel>(await daoMarca.Listar<MarcaModel>());
            Marcas.Insert(0, new MarcaModel(GetResource.GetString("marca_nao_selecionada")));
        }

        public override async void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CodBarra":
                    var result = await daoProduto.ListarPorId(Produto.CodBarra);

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
                _result = await daoProduto.Inserir(Produto);

                AposInserirBDEventArgs e2 = new AposInserirBDEventArgs()
                {
                    InseridoComSucesso = _result,
                    MensagemSucesso = "Produto Inserido com Sucesso",
                    MensagemErro = "Erro ao Inserir Produto",
                    ObjetoSalvo = Produto,
                    CouchDbResponse = e.CouchDbResponse
                };

                ChamaAposInserirNoBD(e2);
            }
        }
    }
}
