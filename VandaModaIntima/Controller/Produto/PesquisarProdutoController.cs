using System.Collections.Generic;
using VandaModaIntima.view.interfaces;
using ModelProduto = VandaModaIntima.Model.Produto;

namespace VandaModaIntima.Controller.Produto
{
    class PesquisarProdutoController
    {
        private readonly IPesquisarView view;
        private readonly ModelProduto produto;

        public PesquisarProdutoController(IPesquisarView view)
        {
            this.view = view;
            produto = new ModelProduto();
        }

        public void PesquisarPorDescricao(string descricao)
        {
            IList<ModelProduto> produtos = produto.ListarPorDescricao(descricao);
            view.AtribuiDataSource(produtos);
        }

        public void PesquisarPorCodigoDeBarra(string codigo)
        {
            IList<ModelProduto> produtos = produto.ListarPorCodigoDeBarra(codigo);
            view.AtribuiDataSource(produtos);
        }

        public void PesquisarPorFornecedor(string fornecedor)
        {
            IList<ModelProduto> produtos = produto.ListarPorFornecedor(fornecedor);
            view.AtribuiDataSource(produtos);
        }

        public void PesquisarPorMarca(string marca)
        {
            IList<ModelProduto> produtos = produto.ListarPorMarca(marca);
            view.AtribuiDataSource(produtos);
        }

        public void FechandoTela()
        {
            produto.DisposeDAO();
        }
    }
}
