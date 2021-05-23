using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOTipoGrade : DAO<TipoGrade>
    {
        public DAOTipoGrade(ISession session) : base(session) { }

        public async override Task<IList<TipoGrade>> Listar()
        {
            var criteria = CriarCriteria();
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }
        public async Task<TipoGrade> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Like("Nome", nome));
            return await criteria.UniqueResultAsync<TipoGrade>();
        }
    }
}
