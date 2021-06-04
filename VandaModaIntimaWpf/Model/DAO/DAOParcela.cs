using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOParcela : DAO<Parcela>
    {
        public DAOParcela(ISession session) : base(session)
        {
        }

        public async Task<IList<Parcela>> ListarPorFuncionarioMesAno(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Adiantamento", "Adiantamento");
            criteria.Add(Restrictions.Eq("Adiantamento.Funcionario", funcionario))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Ano", ano));

            return await Listar(criteria);
        }
        public async Task<IList<Parcela>> ListarPorFuncionarioNaoPagas(Funcionario funcionario)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Adiantamento", "Adiantamento");
            criteria.Add(Restrictions.Eq("Adiantamento.Funcionario", funcionario));
            criteria.Add(Restrictions.Eq("Paga", false));

            return await Listar(criteria);
        }
    }
}
