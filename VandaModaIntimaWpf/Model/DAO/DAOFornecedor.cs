﻿using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOFornecedor : DAO<Fornecedor>
    {
        public DAOFornecedor(ISession session) : base(session) { }

        public async override Task<IList<Fornecedor>> Listar()
        {
            var criteria = CriarCriteria();
            criteria.AddOrder(Order.Asc("Nome"));
            return await Listar(criteria);
        }

        public async Task<IList<Fornecedor>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Nome", "%" + nome + "%"))
                .Add(Restrictions.Like("Fantasia", "%" + nome + "%")));

            criteria.AddOrder(Order.Asc("Nome"));

            return await Listar(criteria);
        }
        public async Task<IList<Fornecedor>> ListarPorCnpj(string cnpj)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", "%" + cnpj + "%")));

            criteria.AddOrder(Order.Asc("Cnpj"));

            return await Listar(criteria);
        }
        public async Task<IList<Fornecedor>> ListarPorEmail(string email)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Email", "%" + email + "%")));

            criteria.AddOrder(Order.Asc("Email"));

            return await Listar(criteria);
        }

        public async Task<Fornecedor> ListarPorIDOuNome(string termo)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", termo))
                .Add(Restrictions.Like("Nome", termo)));

            var result = await Listar(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }
    }
}
