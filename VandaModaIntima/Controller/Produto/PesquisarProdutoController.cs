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

        public void FechandoTela()
        {
            produto.DisposeDAO();
        }
    }
}
