using NHibernate.Criterion;
using System.Collections.Generic;
using VandaModaIntima.Model.DAO;

namespace VandaModaIntima.Model
{
    public class Fornecedor
    {
        public virtual string Cnpj { get; set; }
        public virtual string Nome { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string Email { get; set; }

        public virtual IList<Produto> Produtos { get; set; } = new List<Produto>();

        private IDAO<Fornecedor> daoFornecedor;

        public Fornecedor()
        {
            daoFornecedor = new DAOMySQL<Fornecedor>();
        }

        public Fornecedor(string cnpj, string nome, string fantasia, string email)
        {
            Cnpj = cnpj;
            Nome = nome;
            NomeFantasia = fantasia;
            Email = email;
            daoFornecedor = new DAOMySQL<Fornecedor>();
        }

        public virtual IList<Fornecedor> Listar()
        {
            var criteria = daoFornecedor.CriarCriteria();
            return daoFornecedor.Listar(criteria);
        }

        public virtual IList<Fornecedor> ListarPorNome(string nome)
        {
            var criteria = daoFornecedor.CriarCriteria();

            criteria.Add(Restrictions.Disjunction().Add(Restrictions.Like("Nome", "%" + nome + "%")).Add(Restrictions.Like("NomeFantasia", "%" + nome + "%")));
            //criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
            //criteria.Add(Restrictions.Like("NomeFantasia", "%" + nome + "%"));
            criteria.AddOrder(Order.Asc("Nome"));

            return daoFornecedor.Listar(criteria);
        }

        public virtual void DisposeDAO()
        {
            daoFornecedor.Dispose();
        }
    }
}
