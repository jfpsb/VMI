using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOTipoContagem : DAOMySQL<TipoContagem>
    {
        public DAOTipoContagem(ISession session) : base(session) { }

        public async Task<IList<TipoContagem>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Nome", nome));

            return await Listar(criteria);
        }

        public async override Task<TipoContagem> ListarPorId(params object[] id)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Id", id[0]));

            var result = await Listar(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }
    }
}
