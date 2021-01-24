using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOBonusMensal : DAO
    {
        public DAOBonusMensal(NHibernate.ISession session) : base(session)
        {
        }

        public async Task<IList<BonusMensal>> ListarBonusMensais(Funcionario funcionario)
        {
            var criteria = CriarCriteria<BonusMensal>();

            criteria.Add(Restrictions.Eq("Funcionario", funcionario));

            return await Listar<BonusMensal>(criteria);
        }
        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<BonusMensal>(id);
        }
    }
}
