using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFerias : DAO<Model.Ferias>
    {
        public DAOFerias(ISession session) : base(session)
        {
        }

        public async Task<Ferias> GetFeriasPorFuncionarioMesAno(int mes, int ano, Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.Add(Expression.Sql("YEAR({alias}.Inicio) = ?", ano, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.Inicio) = ?", mes, NHibernateUtil.Int32));
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Deletado", false));

                return await criteria.UniqueResultAsync<Ferias>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "retorna ferias por mes ano funcionario");
                throw new Exception($"Erro ao retornar férias usando mês, ano e funcionário. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
