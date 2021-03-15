using NHibernate;
using System;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOTipoHoraExtra : DAO
    {
        public DAOTipoHoraExtra(ISession session) : base(session)
        {
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.LoadAsync<TipoHoraExtra>(id);
        }
    }
}
