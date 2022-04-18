using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    class DAOEntradaDeMercadoria : DAO<EntradaDeMercadoria>
    {
        public DAOEntradaDeMercadoria(ISession session) : base(session)
        {
        }

        public async Task<IList<EntradaDeMercadoria>> ListarPorMesLoja(DateTime periodo, Loja loja)
        {
            try
            {
                var criteria = CriarCriteria();

                if (loja.Cnpj != null)
                    criteria.Add(Restrictions.Eq("Loja", loja));

                criteria.Add(Expression.Sql("YEAR({alias}.Data) = ?", periodo.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.Data) = ?", periodo.Month, NHibernateUtil.Int32));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de entrada de mercadoria por mês e loja");
                throw new Exception($"Erro ao listar entradas de mercadoria por mês e loja. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
