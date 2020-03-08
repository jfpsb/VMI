using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOMarca : DAO
    {
        public DAOMarca(ISession session) : base(session) { }

        public async Task<IList<Marca>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria<Marca>();
            criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
            criteria.AddOrder(Order.Asc("Nome"));

            return await Listar<Marca>(criteria);
        }

        /// <summary>
        /// Pesquisa a Marca
        /// </summary>
        /// <param name="id">Nome da Marca</param>
        /// <returns>Retorna a Marca Encontrada, Senão, Null</returns>
        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<Marca>(id);
        }
    }
}
