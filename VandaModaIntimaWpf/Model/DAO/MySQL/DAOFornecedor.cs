using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOFornecedor : DAOMySQL<Fornecedor>
    {
        public DAOFornecedor(ISession session) : base(session) { }
        public override Fornecedor ListarPorId(object id)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Cnpj", id));

            var result = Listar(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }

        public IList<Fornecedor> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Nome", "%" + nome + "%"))
                .Add(Restrictions.Like("NomeFantasia", "%" + nome + "%")));

            criteria.AddOrder(Order.Asc("Nome"));

            return Listar(criteria);
        }
        public IList<Fornecedor> ListarPorCnpj(string cnpj)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", "%" + cnpj + "%")));

            criteria.AddOrder(Order.Asc("Cnpj"));

            return Listar(criteria);
        }
        public IList<Fornecedor> ListarPorEmail(string email)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Email", "%" + email + "%")));

            criteria.AddOrder(Order.Asc("Email"));

            return Listar(criteria);
        }

        public Fornecedor ListarPorIDOuNome(string termo)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", termo))
                .Add(Restrictions.Like("Nome", termo)));

            var result = Listar(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }
    }
}
