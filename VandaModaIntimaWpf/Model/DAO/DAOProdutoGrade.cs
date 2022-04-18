using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOProdutoGrade : DAO<ProdutoGrade>
    {
        public DAOProdutoGrade(ISession session) : base(session)
        {
        }

        public async Task<IList<ProdutoGrade>> ListarComLucroValido()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Not(Restrictions.Eq("PrecoCusto", 0.0)));
                criteria.Add(Restrictions.Gt(Projections.SqlProjection("preco_venda - preco_custo", new string[] { "dif_preco" }, new NHibernate.Type.IType[] { NHibernateUtil.Boolean }), 0.0));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar lucros de grade");
                throw new Exception($"Erro ao listar lucros das grades. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
