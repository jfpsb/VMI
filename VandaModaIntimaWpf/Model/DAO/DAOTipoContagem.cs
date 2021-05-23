using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOTipoContagem : DAO<TipoContagem>
    {
        public DAOTipoContagem(ISession session) : base(session) { }

        public async Task<IList<TipoContagem>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("Nome", nome));
            return await Listar(criteria);
        }
    }
}
