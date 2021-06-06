using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOBonusMensal : DAO<BonusMensal>
    {
        public DAOBonusMensal(NHibernate.ISession session) : base(session)
        {
        }

        public async Task<IList<BonusMensal>> ListarPorFuncionario(Funcionario funcionario)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("Funcionario", funcionario));
            return await Listar(criteria);
        }
    }
}
