using NHibernate.Criterion;
using System.Collections.Generic;
using VandaModaIntima.Model.DAO;

namespace VandaModaIntima.Model
{
    public class Produto
    {
        public virtual string Cod_Barra { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Marca Marca { get; set; }
        public virtual string Descricao { get; set; }
        public virtual double Preco { get; set; }

        public virtual IList<string> Codigos { get; set; } = new List<string>();

        private IDAO<Produto> daoProduto;

        public Produto()
        {
            daoProduto = new DAOMySQL<Produto>();
        }

        public virtual string FornecedorNome
        {
            get
            {
                if (Fornecedor != null)
                    return Fornecedor.Nome;

                return "Não Há Fornecedor";
            }
        }

        public virtual string MarcaNome
        {
            get
            {
                if (Marca != null)
                    return Marca.Nome;

                return "Não Há Marca";
            }
        }

        public virtual IList<Produto> Listar()
        {
            var criteria = daoProduto.CriarCriteria();
            return daoProduto.Listar(criteria);
        }



        public virtual IList<Produto> ListarPorDescricao(string descricao)
        {
            var criteria = daoProduto.CriarCriteria();

            criteria.Add(Restrictions.Like("Descricao", "%" + descricao + "%"));
            criteria.AddOrder(Order.Asc("Descricao"));

            return daoProduto.Listar(criteria);
        }

        public virtual void DisposeDAO()
        {
            daoProduto.Dispose();
        }
    }
}
