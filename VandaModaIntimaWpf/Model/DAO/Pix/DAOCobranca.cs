using NHibernate;

namespace VandaModaIntimaWpf.Model.DAO.Pix
{
    public class DAOCobranca : DAO<Model.Pix.Cobranca>
    {
        public DAOCobranca(ISession session) : base(session)
        {
        }
    }
}
