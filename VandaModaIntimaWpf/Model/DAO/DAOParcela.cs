using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOParcela : DAO
    {
        public DAOParcela(ISession session) : base(session)
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
