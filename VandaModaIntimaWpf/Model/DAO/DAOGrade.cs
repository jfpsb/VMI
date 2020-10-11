using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOGrade : DAO
    {
        public DAOGrade(ISession session) : base(session) { }
        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<Grade>(id);
        }
        public async Task<IList<Grade>> ListarPorTipoGrade(TipoGrade tipoGrade)
        {
            var criteria = CriarCriteria<Grade>();
            criteria.CreateAlias("TipoGrade", "TipoGrade");
            criteria.Add(Restrictions.Eq("TipoGrade.Id", tipoGrade.Id));
            criteria.AddOrder(Order.Asc("Nome"));
            criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());
            return await Listar<Grade>(criteria);
        }

        public async Task<Grade> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria<Grade>();
            criteria.Add(Restrictions.Like("Nome", nome));
            return await criteria.UniqueResultAsync<Grade>();
        }
    }
}
