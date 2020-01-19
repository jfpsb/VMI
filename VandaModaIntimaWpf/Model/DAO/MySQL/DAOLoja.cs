using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOLoja : DAOMySQL<Loja>
    {
        public DAOLoja(ISession _session) : base(_session) { }
        public override async Task<Loja> ListarPorId(object id)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Cnpj", id));

            var result = await Listar(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }
        public async Task<IList<Loja>> ListarMatrizes()
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.IsNull("Matriz"));
            return await Listar(criteria);
        }

        public async Task<IList<Loja>> ListarPorCnpj(string termo)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", "%" + termo + "%")));

            criteria.AddOrder(Order.Asc("Cnpj"));

            return await Listar(criteria);
        }

        public async Task<IList<Loja>> ListarPorNome(string termo)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Nome", "%" + termo + "%")));

            criteria.AddOrder(Order.Asc("Nome"));

            return await Listar(criteria);
        }
    }
}
