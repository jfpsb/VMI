using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOContagemProduto : DAO
    {
        public DAOContagemProduto(ISession session) : base(session) { }

        public async Task<IList<ContagemProduto>> ListarPorContagemGroupByProduto(Contagem contagem)
        {
            var criteria = CriarCriteria<ContagemProduto>();

            criteria.Add(Restrictions.Eq("Contagem", contagem));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Sum("Quant"), "Quant")
                .Add(Projections.Property("Id"), "Id")
                .Add(Projections.Property("Contagem"), "Contagem")
                .Add(Projections.GroupProperty("Produto"), "Produto"));

            return await Listar<ContagemProduto>(criteria);
        }
        /// <summary>
        /// Pesquisa a Contagem De Produto Baseado na Id Informada
        /// </summary>
        /// <param name="id">Id da Contagem de Produto</param>
        /// <returns>Retorna Contagem de Produto Encontrada, Senão, Null</returns>
        public async override Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<ContagemProduto>(id);
        }
    }
}
