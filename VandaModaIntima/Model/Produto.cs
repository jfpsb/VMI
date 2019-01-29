using NHibernate.Criterion;
using NHibernate.Transform;
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

        public virtual bool Salvar(Produto produto)
        {
            bool result = daoProduto.Inserir(produto);

            if (result)
                return true;

            return false;
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

        public virtual IList<Produto> ListarPorCodigoDeBarra(string codigo)
        {
            var criteria = daoProduto.CriarCriteria();

            criteria.CreateAlias("Codigos", "CodBarraFornecedor", NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cod_Barra", "%" + codigo + "%"))
                //Tem que usar 'elements' porque no mapa de Produto os códigos estão mapeados como element
                .Add(Restrictions.Like("CodBarraFornecedor.elements", "%" + codigo + "%")));

            //Por causa do groupby eu tenho que especificar as propriedades que quero recuperar no select
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Property("Cod_Barra"), "Cod_Barra")
                .Add(Projections.Property("Descricao"), "Descricao")
                .Add(Projections.Property("Preco"), "Preco")
                .Add(Projections.Property("Fornecedor"), "Fornecedor")
                .Add(Projections.Property("Marca"), "Marca")
                .Add(Projections.GroupProperty("Cod_Barra")));

            criteria.AddOrder(Order.Asc("Cod_Barra"));

            criteria.SetResultTransformer(Transformers.AliasToBean<Produto>());

            return daoProduto.Listar(criteria);
        }

        public virtual IList<Produto> ListarPorFornecedor(string fornecedor)
        {
            var criteria = daoProduto.CriarCriteria();

            criteria.CreateAlias("Fornecedor", "Fornecedor");

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Fornecedor.Nome", "%" + fornecedor + "%"))
                .Add(Restrictions.Like("Fornecedor.NomeFantasia", "%" + fornecedor + "%")));

            return daoProduto.Listar(criteria);
        }

        public virtual IList<Produto> ListarPorMarca(string marca)
        {
            var criteria = daoProduto.CriarCriteria();

            criteria.CreateAlias("Marca", "Marca");

            criteria.Add(Restrictions.Like("Marca.Nome", "%" + marca + "%"));

            return daoProduto.Listar(criteria);
        }

        public virtual void DisposeDAO()
        {
            daoProduto.Dispose();
        }
    }
}
