using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFuncionario : DAO
    {
        public DAOFuncionario(ISession session) : base(session) { }

        public async override Task<IList<E>> Listar<E>()
        {
            var criteria = CriarCriteria<Funcionario>();
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar<E>(criteria);
        }
        public async Task<IList<Funcionario>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria<Funcionario>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Nome", "%" + nome + "%")));

            criteria.AddOrder(Order.Asc("Nome"));

            return await Listar<Funcionario>(criteria);
        }
        public async Task<IList<Funcionario>> ListarPorCpf(string cnpj)
        {
            var criteria = CriarCriteria<Funcionario>();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cpf", "%" + cnpj + "%")));

            criteria.AddOrder(Order.Asc("Nome"));

            return await Listar<Funcionario>(criteria);
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<Funcionario>(id);
        }
    }
}
