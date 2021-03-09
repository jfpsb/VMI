using NHibernate;
using System;
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

        public override Task<object> ListarPorId(object id)
        {
            throw new NotImplementedException();
        }
    }
}
