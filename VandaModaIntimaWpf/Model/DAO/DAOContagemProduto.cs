using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOContagemProduto : DAO<ContagemProduto>
    {
        public DAOContagemProduto(ISession session) : base(session) { }

        public async Task<IList<ContagemProduto>> ListarPorContagemGroupByProduto(Contagem contagem)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Contagem", contagem));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Sum("Quant"), "Quant")
                .Add(Projections.Property("Id"), "Id")
                .Add(Projections.Property("Contagem"), "Contagem")
                .Add(Projections.GroupProperty("Produto"), "Produto"));

            return await Listar(criteria);
        }
    }
}
