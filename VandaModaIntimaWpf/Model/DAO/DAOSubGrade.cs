using NHibernate;
using System;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOSubGrade : DAO
    {
        public DAOSubGrade(ISession session) : base(session)
        {
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public override Task<object> ListarPorId(object id)
        {
            throw new NotImplementedException();
        }
    }
}
