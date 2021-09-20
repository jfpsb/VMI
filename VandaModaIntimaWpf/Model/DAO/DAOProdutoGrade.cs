using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOProdutoGrade : DAO<ProdutoGrade>
    {
        public DAOProdutoGrade(ISession session) : base(session)
        {
        }

        public async Task<IList<ProdutoGrade>> ListarComLucroValido()
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Not(Restrictions.Eq("PrecoCusto", 0.0)));
            criteria.Add(Restrictions.Gt(Projections.SqlProjection("preco_venda - preco_custo", new string[] { "dif_preco" }, new NHibernate.Type.IType[] { NHibernateUtil.Boolean }), 0.0));

            return await Listar(criteria);
        }
    }
}
