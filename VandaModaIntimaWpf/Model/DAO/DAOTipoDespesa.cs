using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOTipoDespesa : DAO<TipoDespesa>
    {
        public DAOTipoDespesa(ISession session) : base(session)
        {
        }

        public async override Task<IList<TipoDespesa>> Listar()
        {
            var criteria = CriarCriteria();
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }
    }
}
