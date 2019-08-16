using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOMarca : DAOMySQL<Marca>
    {
        public DAOMarca(ISession session) : base(session) { }

        public virtual IList<Marca> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
            criteria.AddOrder(Order.Asc("Nome"));

            return Listar(criteria);
        }
        public override Marca ListarPorId(object id)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Nome", id));

            var result = Listar(criteria);

            if (result.Count == 0)
                return null;

            return result[0];
        }
    }
}
