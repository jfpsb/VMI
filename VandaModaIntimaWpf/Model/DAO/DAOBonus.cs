using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOBonus : DAO
    {
        public DAOBonus(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Lista os bônus de um funcionário da folha atual e os bônus que são pagos todo mês.
        /// </summary>
        /// <param name="funcionario">Funcionário que terá os bônus pesquisados</param>
        /// <returns></returns>
        public async Task<IList<Bonus>> ListarPorFuncionarioComBonusMensal(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria<Bonus>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Eq("PagamentoMensal", true))
                .Add(Restrictions.Conjunction()
                .Add(Restrictions.Eq("Funcionario", funcionario))
                .Add(Restrictions.Eq("MesReferencia", mes))
                .Add(Restrictions.Eq("AnoReferencia", ano))));

            return await Listar<Bonus>(criteria);
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<Bonus>(id);
        }
    }
}
