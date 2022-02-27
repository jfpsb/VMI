using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOProduto : DAO<Produto>
    {
        public DAOProduto(ISession session) : base(session)
        {
        }

        public async Task<IList<Produto>> ListarPorDescricao(string descricao)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
            criteria.Add(Restrictions.Like("Descricao", "%" + descricao + "%"));
            criteria.AddOrder(Order.Asc("Descricao"));
            criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());

            return await ListarComNovaSession(criteria);
        }

        public async Task<IList<Produto>> ListarPorDescricaoCodigoDeBarra(string termo)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("CodBarra", "%" + termo + "%"))
                .Add(Restrictions.Like("Grades.CodBarra", "%" + termo + "%"))
                .Add(Restrictions.Like("Grades.CodBarraAlternativo", "%" + termo + "%"))
                .Add(Restrictions.Like("Descricao", "%" + termo + "%")));

            //criteria.SetProjection(Projections.GroupProperty("CodBarra"));
            criteria.AddOrder(Order.Asc("Descricao"));
            criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());

            return await ListarComNovaSession(criteria);
        }

        public async Task<IList<Produto>> ListarPorCodigoDeBarra(string codigo)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
            criteria.Add(Restrictions.Like("CodBarra", "%" + codigo + "%"));
            criteria.AddOrder(Order.Asc("CodBarra"));
            criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());

            return await ListarComNovaSession(criteria);
        }
        public async Task<Produto> ListarPorCodigoDeBarraUnico(string codigo)
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    Produto p = await session.CreateQuery("from Produto where CodBarra = :cod and Deletado = false")
                        .SetString("cod", codigo).UniqueResultAsync<Produto>();

                    await tx.CommitAsync();

                    return p;
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }
        public async Task<IList<Produto>> ListarPorFornecedor(string fornecedor)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
            criteria.CreateAlias("Fornecedor", "Fornecedor");

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Fornecedor.Nome", "%" + fornecedor + "%"))
                .Add(Restrictions.Like("Fornecedor.Fantasia", "%" + fornecedor + "%")));

            return await ListarComNovaSession(criteria);
        }
        public async Task<IList<Produto>> ListarPorMarca(string marca)
        {
            var criteria = CriarCriteria();
            //criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
            criteria.CreateAlias("Marca", "Marca");
            criteria.Add(Restrictions.Like("Marca.Nome", "%" + marca + "%"));
            return await ListarComNovaSession(criteria);
        }
    }
}
