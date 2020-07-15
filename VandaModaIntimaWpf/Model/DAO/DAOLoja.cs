using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOLoja : DAO
    {
        public DAOLoja(ISession _session) : base(_session) { }

        /// <summary>
        /// Pesquisa a Loja
        /// </summary>
        /// <param name="id">Cnpj da Loja</param>
        /// <returns>Retorna a Loja Encontrada, Senão, Null</returns>
        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<Loja>(id);
        }
        public async Task<IList<Loja>> ListarMatrizes()
        {
            var criteria = CriarCriteria<Loja>();
            criteria.Add(Restrictions.IsNull("Matriz"));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar<Loja>(criteria);
        }

        public async Task<IList<Loja>> ListarPorCnpj(string termo)
        {
            var criteria = CriarCriteria<Loja>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", "%" + termo + "%")));

            criteria.AddOrder(Order.Asc("Cnpj"));

            return await Listar<Loja>(criteria);
        }
        public async Task<IList<Loja>> ListarExcetoDeposito()
        {
            var criteria = CriarCriteria<Loja>();
            criteria.Add(Restrictions.Not(Restrictions.Eq("Cnpj", "000000000")));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar<Loja>(criteria);
        }
        public async Task<IList<Loja>> ListarPorNome(string termo)
        {
            var criteria = CriarCriteria<Loja>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Nome", "%" + termo + "%")));

            criteria.AddOrder(Order.Asc("Nome"));

            return await Listar<Loja>(criteria);
        }

        public override int GetMaxId()
        {
            throw new System.NotImplementedException();
        }
    }
}
