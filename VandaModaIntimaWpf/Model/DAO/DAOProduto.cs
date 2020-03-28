using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOProduto : DAO
    {
        public DAOProduto(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Retorna o Produto
        /// </summary>
        /// <param name="id">Código de Barras do Produto</param>
        /// <returns>Retorna o Produto Encontrado, Senão, Null</returns>
        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<Produto>(id);
        }

        public async Task<IList<Produto>> ListarPorDescricao(string descricao)
        {
            var criteria = CriarCriteria<Produto>();

            criteria.Add(Restrictions.Like("Descricao", "%" + descricao + "%"));
            criteria.AddOrder(Order.Asc("Descricao"));

            return await Listar<Produto>(criteria);
        }

        public async Task<IList<Produto>> ListarPorDescricaoCodigoDeBarra(string termo)
        {
            var criteria = CriarCriteria<Produto>();

            criteria.CreateAlias("Codigos", "Codigos", NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("CodBarra", "%" + termo + "%"))
                //Tem que usar 'elements' porque no mapa de Produto os códigos estão mapeados como element
                .Add(Restrictions.Like("Codigos.elements", "%" + termo + "%"))
                .Add(Restrictions.Like("Descricao", termo)));

            //Por causa do groupby eu tenho que especificar as propriedades que quero recuperar no select
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Property("CodBarra"), "CodBarra")
                .Add(Projections.Property("Descricao"), "Descricao")
                .Add(Projections.Property("Preco"), "Preco")
                .Add(Projections.Property("Fornecedor"), "Fornecedor")
                .Add(Projections.Property("Marca"), "Marca")
                .Add(Projections.Property("Ncm"), "Ncm")
                .Add(Projections.GroupProperty("CodBarra")));

            criteria.AddOrder(Order.Asc("CodBarra"));

            criteria.SetResultTransformer(Transformers.AliasToBean<Produto>());

            return await Listar<Produto>(criteria);
        }

        public async Task<IList<Produto>> ListarPorCodigoDeBarra(string codigo)
        {
            var criteria = CriarCriteria<Produto>();

            criteria.CreateAlias("Codigos", "Codigos", NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("CodBarra", "%" + codigo + "%"))
                //Tem que usar 'elements' porque no mapa de Produto os códigos estão mapeados como element
                .Add(Restrictions.Like("Codigos.elements", "%" + codigo + "%")));

            //Por causa do groupby eu tenho que especificar as propriedades que quero recuperar no select
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Property("CodBarra"), "CodBarra")
                .Add(Projections.Property("Descricao"), "Descricao")
                .Add(Projections.Property("Preco"), "Preco")
                .Add(Projections.Property("Fornecedor"), "Fornecedor")
                .Add(Projections.Property("Marca"), "Marca")
                .Add(Projections.Property("Ncm"), "Ncm")
                .Add(Projections.GroupProperty("CodBarra")));

            criteria.AddOrder(Order.Asc("CodBarra"));

            criteria.SetResultTransformer(Transformers.AliasToBean<Produto>());

            return await Listar<Produto>(criteria);
        }

        public async Task<IList<Produto>> ListarPorFornecedor(string fornecedor)
        {
            var criteria = CriarCriteria<Produto>();

            criteria.CreateAlias("Fornecedor", "Fornecedor");

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Fornecedor.Nome", "%" + fornecedor + "%"))
                .Add(Restrictions.Like("Fornecedor.Fantasia", "%" + fornecedor + "%")));

            return await Listar<Produto>(criteria);
        }

        public async Task<IList<Produto>> ListarPorMarca(string marca)
        {
            var criteria = CriarCriteria<Produto>();

            criteria.CreateAlias("Marca", "Marca");
            criteria.Add(Restrictions.Like("Marca.Nome", "%" + marca + "%"));

            return await Listar<Produto>(criteria);
        }

        public override int GetMaxId()
        {
            ISQLQuery query = session.CreateSQLQuery("SELECT max(CAST(cod_barra as SIGNED)) from produto;");
            return int.Parse(query.UniqueResult().ToString());
        }
    }
}
