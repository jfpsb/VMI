using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOLoja : DAO<Loja>
    {
        public DAOLoja(ISession _session) : base(_session) { }

        public override Task<IList<Loja>> Listar()
        {
            var criteria = CriarCriteria();
            criteria.AddOrder(Order.Asc("Nome"));
            criteria.SetCacheable(true);
            criteria.SetCacheMode(CacheMode.Normal);
            return base.Listar();
        }

        public async Task<IList<Loja>> ListarMatrizes()
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.IsNull("Matriz"));
            criteria.Add(Restrictions.Not(Restrictions.Eq("Cnpj", "000000000")));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }

        public async Task<IList<Loja>> ListarPorCnpj(string termo)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Disjunction().Add(Restrictions.Like("Cnpj", "%" + termo + "%")));
            criteria.AddOrder(Order.Asc("Cnpj"));

            return await Listar(criteria);
        }
        public async Task<IList<Loja>> ListarExcetoDeposito()
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Not(Restrictions.Eq("Cnpj", "000000000")));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }
        public async Task<IList<Loja>> ListarPorNome(string termo)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Disjunction().Add(Restrictions.Like("Nome", "%" + termo + "%")));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }
    }
}
