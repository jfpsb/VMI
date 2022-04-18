using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOHoraExtra : DAO<HoraExtra>
    {
        public DAOHoraExtra(ISession session) : base(session)
        {
        }

        public async Task<HoraExtra> ListarPorAnoMesFuncionarioTipo(int ano, int mes, Funcionario funcionario, TipoHoraExtra tipo)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Ano", ano))
                    .Add(Restrictions.Eq("Mes", mes))
                    .Add(Restrictions.Eq("Funcionario", funcionario))
                    .Add(Restrictions.Eq("TipoHoraExtra", tipo));

                return await criteria.UniqueResultAsync<HoraExtra>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar hora extra data funcionario tipo");
                throw new Exception($"Erro ao listar horas extras. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<HoraExtra>> ListarPorAnoMesFuncionario(int ano, int mes, Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Ano", ano))
                    .Add(Restrictions.Eq("Mes", mes))
                    .Add(Restrictions.Eq("Funcionario", funcionario));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar hora extra data funcionario");
                throw new Exception($"Erro ao listar horas extras. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
