using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOAdiantamento : DAO
    {
        public DAOAdiantamento(ISession session) : base(session)
        {
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.LoadAsync<Adiantamento>(id);
        }
    }
}
