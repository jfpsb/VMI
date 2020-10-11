using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
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
        public async override Task<IList<E>> Listar<E>()
        {
            var criteria = CriarCriteria<E>();
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar<E>(criteria);
        }
        public async Task<TipoGrade> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria<TipoGrade>();
            criteria.Add(Restrictions.Like("Nome", nome));
            return await criteria.UniqueResultAsync<TipoGrade>();
        }
    }
}
