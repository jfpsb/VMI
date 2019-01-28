using System.Collections.Generic;
using VandaModaIntima.view.interfaces;
using VandaModaIntima.View.Produto;
using ModelFornecedor = VandaModaIntima.Model.Fornecedor;
using ModelMarca = VandaModaIntima.Model.Marca;
using ModelProduto = VandaModaIntima.Model.Produto;

namespace VandaModaIntima.Controller.Produto
{
    class CadastrarProdutoController
    {
        private ModelProduto produto;
        private readonly ModelFornecedor fornecedor;
        private readonly ModelMarca marca;
        private ICadastrarView view;

        public CadastrarProdutoController(ICadastrarView view)
        {
            this.view = view;
            produto = new ModelProduto();
            fornecedor = new ModelFornecedor();
            marca = new ModelMarca();
        }

        public void Cadastrar(string cod_barra, ModelFornecedor fornecedor, ModelMarca marca, string descricao, string preco)
        {
            double preco_n;

            if (cod_barra == string.Empty)
            {
                view.MensagemAviso("Código de Barras Não Pode Ser Vazio");
                return;
            }

            if (descricao == string.Empty)
            {
                view.MensagemAviso("Descrição Não Pode Ser Vazia");
                return;
            }

            if (preco == string.Empty)
            {
                view.MensagemAviso("Preço Não Pode Ser Vazio");
                return;
            }

            if (!double.TryParse(preco, out preco_n))
            {
                view.MensagemAviso("Digite Um Preço Válido");
                return;
            }

            produto.Cod_Barra = cod_barra;
            produto.Fornecedor = fornecedor;
            produto.Marca = marca;
            produto.Descricao = descricao;
            produto.Preco = preco_n;

            bool result = produto.Salvar(produto);

            if (result)
            {
                view.MensagemAviso("Produto Cadastrado Com Sucesso");
                view.AposCadastro();
                view.LimparCampos();
            }
            else
            {
                view.MensagemErro("Produto Não Foi Cadastrado");
            }

            //Reseta produto
            produto = new ModelProduto();
        }

        public void PesquisarFornecedor()
        {
            IList<ModelFornecedor> fornecedores = fornecedor.Listar();
            fornecedores.Insert(0, new ModelFornecedor("0", "ESCREVA O FORNECEDOR", null, null));
            ((CadastrarProduto)view).AtribuiDataSourceCmbFornecedor(fornecedores);
        }

        public void PesquisarMarca()
        {
            IList<ModelMarca> marcas = marca.Listar();
            marcas.Insert(0, new ModelMarca(0, "ESCREVA A MARCA"));
            ((CadastrarProduto)view).AtribuiDataSourceCmbMarca(marcas);
        }
    }
}
