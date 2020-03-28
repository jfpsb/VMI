using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOFornecedor : DAO
    {
        public DAOFornecedor(ISession session) : base(session) { }

        public async override Task<IList<E>> Listar<E>()
        {
            var criteria = CriarCriteria<Fornecedor>();
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar<E>(criteria);
        }

        /// <summary>
        /// Pesquisa o Fornecedor
        /// </summary>
        /// <param name="id">Cnpj do Fornecedor</param>
        /// <returns>Retorna o Fornecedor Encontrado, Senão, Null</returns>
        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<Fornecedor>(id);
        }

        public async Task<IList<Fornecedor>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria<Fornecedor>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Nome", "%" + nome + "%"))
                .Add(Restrictions.Like("Fantasia", "%" + nome + "%")));

            criteria.AddOrder(Order.Asc("Nome"));

            return await Listar<Fornecedor>(criteria);
        }
        public async Task<IList<Fornecedor>> ListarPorCnpj(string cnpj)
        {
            var criteria = CriarCriteria<Fornecedor>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", "%" + cnpj + "%")));

            criteria.AddOrder(Order.Asc("Cnpj"));

            return await Listar<Fornecedor>(criteria);
        }
        public async Task<IList<Fornecedor>> ListarPorEmail(string email)
        {
            var criteria = CriarCriteria<Fornecedor>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Email", "%" + email + "%")));

            criteria.AddOrder(Order.Asc("Email"));

            return await Listar<Fornecedor>(criteria);
        }

        public async Task<Fornecedor> ListarPorIDOuNome(string termo)
        {
            var criteria = CriarCriteria<Fornecedor>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", termo))
                .Add(Restrictions.Like("Nome", termo)));

            var result = await Listar<Fornecedor>(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }
    }
}
