using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOHoraExtra : DAO
    {
        public DAOHoraExtra(ISession session) : base(session)
        {
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.LoadAsync<HoraExtra>(id);
        }

        public async Task<HoraExtra> ListarPorAnoMesFuncionarioTipo(int ano, int mes, Funcionario funcionario, TipoHoraExtra tipo)
        {
            var criteria = CriarCriteria<HoraExtra>();
            criteria.Add(Restrictions.Eq("Ano", ano))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Funcionario", funcionario))
                .Add(Restrictions.Eq("TipoHoraExtra", tipo));

            return await criteria.UniqueResultAsync<HoraExtra>();
        }

        public async Task<IList<HoraExtra>> ListarPorAnoMesFuncionario(int ano, int mes, Funcionario funcionario)
        {
            var criteria = CriarCriteria<HoraExtra>();
            criteria.Add(Restrictions.Eq("Ano", ano))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Funcionario", funcionario));

            return await Listar<HoraExtra>(criteria);
        }
    }
}
