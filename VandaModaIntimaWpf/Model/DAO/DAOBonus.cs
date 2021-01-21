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

            var conjunction1 = Restrictions.Conjunction().Add(Restrictions.Eq("MesReferencia", mes)).Add(Restrictions.Eq("AnoReferencia", ano));
            var disjunction1 = Restrictions.Disjunction().Add(conjunction1).Add(Restrictions.Eq("PagamentoMensal", true));

            criteria.Add(disjunction1).Add(Restrictions.Eq("Funcionario", funcionario));

            return await Listar<Bonus>(criteria);
        }

        public async Task<Bonus> ListarHoraExtra100(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria<Bonus>();

            criteria.Add(Restrictions.Like("Descricao", "HORA EXTRA C/100%"))
                .Add(Restrictions.Eq("MesReferencia", mes))
                .Add(Restrictions.Eq("AnoReferencia", ano))
                .Add(Restrictions.Eq("Funcionario", funcionario));

            return await criteria.UniqueResultAsync<Bonus>();
        }

        public async Task<Bonus> ListarHoraExtra55(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria<Bonus>();

            criteria.Add(Restrictions.Like("Descricao", "HORA EXTRA C/055%"))
                .Add(Restrictions.Eq("MesReferencia", mes))
                .Add(Restrictions.Eq("AnoReferencia", ano))
                .Add(Restrictions.Eq("Funcionario", funcionario));

            return await criteria.UniqueResultAsync<Bonus>();
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
