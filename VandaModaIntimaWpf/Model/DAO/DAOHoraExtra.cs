using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOHoraExtra : DAO<HoraExtra>
    {
        public DAOHoraExtra(ISession session) : base(session)
        {
        }

        public async Task<HoraExtra> ListarPorAnoMesFuncionarioTipo(int ano, int mes, Funcionario funcionario, TipoHoraExtra tipo)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("Ano", ano))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Funcionario", funcionario))
                .Add(Restrictions.Eq("TipoHoraExtra", tipo));

            return await criteria.UniqueResultAsync<HoraExtra>();
        }

        public async Task<IList<HoraExtra>> ListarPorAnoMesFuncionario(int ano, int mes, Funcionario funcionario)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("Ano", ano))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Funcionario", funcionario));

            return await Listar(criteria);
        }
    }
}
