using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOMarca : DAOMySQL<Marca>
    {
        public DAOMarca(ISession session) : base(session) { }

        public async Task<IList<Marca>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
            criteria.AddOrder(Order.Asc("Nome"));

            return await Listar(criteria);
        }
        public override async Task<Marca> ListarPorId(params object[] id)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Nome", id[0]));

            var result = await Listar(criteria);

            if (result.Count == 0)
                return null;

            return result[0];
        }
    }
}
