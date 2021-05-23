using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOGrade : DAO<Grade>
    {
        public DAOGrade(ISession session) : base(session) { }
        public async Task<IList<Grade>> ListarPorTipoGrade(TipoGrade tipoGrade)
        {
            var criteria = CriarCriteria();
            criteria.CreateAlias("TipoGrade", "TipoGrade");
            criteria.Add(Restrictions.Eq("TipoGrade.Id", tipoGrade.Id));
            criteria.AddOrder(Order.Asc("Nome"));
            criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());
            return await Listar(criteria);
        }

        public async Task<Grade> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Like("Nome", nome));
            return await criteria.UniqueResultAsync<Grade>();
        }
    }
}
