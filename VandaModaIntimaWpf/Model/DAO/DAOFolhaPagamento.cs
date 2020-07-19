using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFolhaPagamento : DAO
    {
        public DAOFolhaPagamento(ISession session) : base(session)
        {
        }

        public async Task<FolhaPagamento> ListarPorMesAnoFuncionario(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria<FolhaPagamento>();
            criteria.Add(Restrictions.Eq("Funcionario", funcionario))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Ano", ano));

            return (FolhaPagamento)await criteria.UniqueResultAsync();
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public override Task<object> ListarPorId(object id)
        {
            throw new NotImplementedException();
        }
    }
}
