using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.View;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoViewModel : CadastrarProdutoViewModel, IEditarViewModel
    {
        private bool IsEditted = false;
        public override async void Cadastrar(object parameter)
        {
            if (Produto.Fornecedor != null && Produto.Fornecedor.Cnpj.Equals("0"))
                Produto.Fornecedor = null;

            if (Produto.Marca != null && Produto.Marca.Equals("SELECIONE UMA MARCA"))
                Produto.Marca = null;

            var result = IsEditted = await daoProduto.Atualizar(produtoModel);

            if (result)
            {
                ((IMessageable)parameter).MensagemDeAviso("Produto Editado Com Sucesso");
                ((ICloseable)parameter).Close();
            }
            else
            {
                ((IMessageable)parameter).MensagemDeErro("Erro ao Editar Produto");
            }
        }

        public async void PassaId(object Id)
        {
            Produto = await _session.LoadAsync<ProdutoModel>(Id);
        }

        public bool EdicaoComSucesso()
        {
            return IsEditted;
        }

        public new ProdutoModel Produto
        {
            get { return produtoModel; }
            set
            {
                produtoModel = value;
                OnPropertyChanged("Produto");
                OnPropertyChanged("FornecedorComboBox");
                OnPropertyChanged("MarcaComboBox");
            }
        }

        public FornecedorModel FornecedorComboBox
        {
            get
            {
                if (Produto.Fornecedor == null)
                {
                    Produto.Fornecedor = new FornecedorModel("SELECIONE UM FORNECEDOR");
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
                    Produto.Marca = new MarcaModel("SELECIONE UMA MARCA");
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
