using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAORepresentante : DAO<Representante>
    {
        public DAORepresentante(ISession session) : base(session)
        {
        }

        public async override Task<IList<Representante>> Listar()
        {
            var criteria = CriarCriteria();
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }

        public async Task<IList<Representante>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Like("Nome", $"%{nome}%"));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }
    }
}
