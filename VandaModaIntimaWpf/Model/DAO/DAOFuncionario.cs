using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFuncionario : DAO<Funcionario>
    {
        public DAOFuncionario(ISession session) : base(session) { }

        public async override Task<IList<Funcionario>> Listar()
        {
            var criteria = CriarCriteria();
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }
        public async Task<IList<Funcionario>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }
        public async Task<IList<Funcionario>> ListarPorCpf(string cnpj)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Like("Cpf", "%" + cnpj + "%"));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }

        public async Task<IList<Funcionario>> ListarQuemRecebePassagem()
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("RecebePassagem", true));
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }
    }
}
