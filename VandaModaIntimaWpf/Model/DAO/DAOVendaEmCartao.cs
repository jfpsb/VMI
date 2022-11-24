using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOVendaEmCartao : DAO<VendaEmCartao>
    {
        public DAOVendaEmCartao(ISession session) : base(session)
        {
        }
    }
}
