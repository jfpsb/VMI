using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOPontoEletronico : DAO<PontoEletronico>
    {
        public DAOPontoEletronico(ISession session) : base(session)
        {
        }

        public async Task<PontoEletronico> ListarPorDiaFuncionario(DateTime dia, Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Dia", dia));
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Deletado", false));
                return await criteria.UniqueResultAsync<PontoEletronico>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar por dia funcionario em DAOPontoEletronico");
                throw new Exception($"Erro ao listar pontos por dia e funcionário. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<PontoEletronico>> ListarPontosPorFuncionarioMes(DateTime data, Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Expression.Sql("YEAR({alias}.Dia) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.Dia) = ?", data.Month, NHibernateUtil.Int32));
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Deletado", false));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar por funcionario mes em DAOPontoEletronico");
                throw new Exception($"Erro ao listar pontos por funcionário e mês. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
