using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.Model.DAO.MySQL;
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

        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }
        public CadastrarProdutoViewModel() : base()
        {
            daoProduto = new DAOProduto(_session);
            daoMarca = new DAOMarca(_session);
            daoFornecedor = new DAOFornecedor(_session);
            produtoModel = new ProdutoModel();

            produtoModel.PropertyChanged += CadastrarViewModel_PropertyChanged;

            GetFornecedores();
            GetMarcas();
        }
        public override bool ValidaModel(object parameter)
        {
            if (string.IsNullOrEmpty(Produto.Cod_Barra) || string.IsNullOrEmpty(Produto.Descricao))
            {
                return false;
            }

            if (Produto.Preco.ToString().Equals(string.Empty) || Produto.Preco == 0)
            {
                return false;
            }

            return true;
        }

        public override async void Cadastrar(object parameter)
        {
            if (Produto.Fornecedor.Cnpj.Equals("0"))
                Produto.Fornecedor = null;

            if (Produto.Marca.Nome.Equals("SELECIONE A MARCA"))
                Produto.Marca = null;

            var result = await daoProduto.Inserir(produtoModel);

            if (result)
            {
                ResetaPropriedades();
                await SetStatusBarSucesso();
                return;
            }

            SetStatusBarErro();
        }

        public override void ResetaPropriedades()
        {
            Produto = new ProdutoModel();
            Produto.Cod_Barra = Produto.Descricao = string.Empty;
            Produto.Preco = 0;
            Produto.Fornecedor = Fornecedores[0];
            Produto.Marca = Marcas[0];
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

        private async void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<FornecedorModel>(await daoFornecedor.Listar());
            Fornecedores.Insert(0, new FornecedorModel("SELECIONE O FORNECEDOR"));
        }

        private async void GetMarcas()
        {
            Marcas = new ObservableCollection<MarcaModel>(await daoMarca.Listar());
            Marcas.Insert(0, new MarcaModel("SELECIONE A MARCA"));
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cod_Barra":
                    var result = daoProduto.ListarPorId(Produto.Cod_Barra);

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
    }
}
