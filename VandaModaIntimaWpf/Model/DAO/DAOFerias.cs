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

        /// <summary>
        /// Consulta no banco de dados as férias registradas que começarão daqui dois meses para fins de geração de documento
        /// de comunicação de férias.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Ferias>> RetornaFeriasParaComunicao()
        {
            try
            {
                var mesConsulta = DateTime.Now.AddMonths(2);

                var criteria = CriarCriteria();
                criteria.Add(Expression.Sql("MONTH({alias}.Inicio) = ?", mesConsulta.Month, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("YEAR({alias}.Inicio) = ?", mesConsulta.Year, NHibernateUtil.Int32));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "retorna ferias daqui dois meses");
                throw new Exception($"Erro ao retornar férias para finsd de geração de documento de comunicação de férias. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
