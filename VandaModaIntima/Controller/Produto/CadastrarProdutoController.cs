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
        private readonly ModelProduto produto;
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

        public void Cadastrar(ModelProduto produto)
        {
            bool result = produto.Salvar(produto);

            if (result)
            {
                view.MensagemAviso("Produto Cadastrado Com Sucesso");
                view.AposCadastro();
                view.LimparCampos();
                //Reseta modelo
                produto = new ModelProduto();
            }
            else
            {
                view.MensagemErro("Produto Não Foi Cadastrado");
            }
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
