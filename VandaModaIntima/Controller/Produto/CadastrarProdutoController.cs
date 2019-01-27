using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntima.view.interfaces;
using ModelProduto = VandaModaIntima.Model.Produto;

namespace VandaModaIntima.Controller.Produto
{
    class CadastrarProdutoController
    {
        ModelProduto produto;
        private ICadastrarView view;

        public CadastrarProdutoController(ICadastrarView view)
        {
            this.view = view;
            produto = new ModelProduto();
        }
    }
}
