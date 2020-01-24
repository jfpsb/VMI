using NHibernate;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOOperadoraCartao : DAOMySQL<OperadoraCartao>
    {
        public DAOOperadoraCartao(ISession session) : base(session) { }
        public async override Task<OperadoraCartao> ListarPorId(params object[] id)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Nome", id[0]));

            var result = await Listar(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }
    }
}
