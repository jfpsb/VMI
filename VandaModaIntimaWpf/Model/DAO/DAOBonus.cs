using NHibernate;
using System;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOBonus : DAO
    {
        public DAOBonus(ISession session) : base(session)
        {
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
