using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOVendaEmCartao : DAO<VendaEmCartao>
    {
        public DAOVendaEmCartao(ISession session) : base(session)
        {
        }

        public async Task<IList<VendaEmCartao>> ListarPorMesPorLoja(DateTime data, Loja loja)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Loja", loja));
                criteria.Add(Expression.Sql("YEAR({alias}.data_hora) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.data_hora) = ?", data.Month, NHibernateUtil.Int32));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar vendas em cartao por mes, loja");
                throw new Exception($"Erro ao listar vendas em cartão por mês e loja. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<VendaEmCartao>> ListarPorMesPorLojaOperadora(DateTime data, Loja loja, OperadoraCartao operadora)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Loja", loja));
                criteria.Add(Restrictions.Eq("OperadoraCartao", operadora));
                criteria.Add(Expression.Sql("YEAR({alias}.data_hora) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.data_hora) = ?", data.Month, NHibernateUtil.Int32));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar vendas em cartao por mes, loja");
                throw new Exception($"Erro ao listar vendas em cartão por mês e loja. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
