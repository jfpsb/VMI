using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class CadastrarProdutoViewModel : ACadastrarViewModel
    {
        protected ProdutoModel produtoModel;
        private FornecedorModel fornecedorModel;
        private MarcaModel marcaModel;
        public ObservableCollection<FornecedorModel> Fornecedores { get; set; }
        public ObservableCollection<MarcaModel> Marcas { get; set; }
        public CadastrarProdutoViewModel() : base()
        {
            produtoModel = new ProdutoModel();
            fornecedorModel = new FornecedorModel();
            marcaModel = new MarcaModel();

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

        public override void Cadastrar(object parameter)
        {
            if (Produto.Fornecedor.Cnpj.Equals("0"))
                Produto.Fornecedor = null;

            if (Produto.Marca.Id == 0)
                Produto.Marca = null;

            var result = produtoModel.Salvar();

            if (result)
            {
                MensagemStatusBar = "Cadastro Realizado Com Sucesso";
                ImagemStatusBar = IMAGEMSUCESSO;
                ResetaPropriedades();
                return;
            }

            MensagemStatusBar = "Erro ao Cadastrar";
            ImagemStatusBar = IMAGEMERRO;
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

        private void GetFornecedores()
        {
            Fornecedores = new ObservableCollection<FornecedorModel>(fornecedorModel.Listar());
            Fornecedores.Insert(0, new FornecedorModel("SELECIONE O FORNECEDOR"));
        }

        private void GetMarcas()
        {
            Marcas = new ObservableCollection<MarcaModel>(marcaModel.Listar());
            Marcas.Insert(0, new MarcaModel("SELECIONE A MARCA"));
        }

        public override void CadastrarViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Cod_Barra":
                    var result = produtoModel.ListarPorId(Produto.Cod_Barra);

                    if (result != null)
                    {
                        VisibilidadeAvisoCodBarra = Visibility.Visible;
                        IsEnabled = false;
                    }
                    else
                    {
                        VisibilidadeAvisoCodBarra = Visibility.Collapsed;
                        IsEnabled = true;
                    }

                    break;
            }
        }
    }
}
