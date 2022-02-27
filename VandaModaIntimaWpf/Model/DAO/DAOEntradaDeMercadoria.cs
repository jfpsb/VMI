using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    class DAOEntradaDeMercadoria : DAO<EntradaDeMercadoria>
    {
        public DAOEntradaDeMercadoria(ISession session) : base(session)
        {
        }
    }
}
