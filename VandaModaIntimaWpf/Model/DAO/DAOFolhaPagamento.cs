using NHibernate;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFolhaPagamento : DAO<FolhaPagamento>
    {
        public DAOFolhaPagamento(ISession session) : base(session)
        {
        }

        public async Task<FolhaPagamento> ListarPorMesAnoFuncionario(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("Funcionario", funcionario))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Ano", ano));

            return (FolhaPagamento)await criteria.UniqueResultAsync();
        }
    }
}
