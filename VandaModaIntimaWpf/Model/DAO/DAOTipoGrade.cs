using NHibernate;
using System;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOTipoGrade : DAO
    {
        public DAOTipoGrade(ISession session) : base(session) { }
        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<TipoGrade>(id);
        }
    }
}
