using NHibernate.Criterion;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.Model
{
    class FornecedorModel
    {
        private IDAO<Fornecedor> dao;
        public Fornecedor fornecedor;

        public FornecedorModel()
        {
            dao = new DAOMySQL<Fornecedor>();
        }

        public virtual IList<Fornecedor> Listar()
        {
            var criteria = dao.CriarCriteria();
            return dao.Listar(criteria);
        }

        public virtual IList<Fornecedor> ListarPorNome(string nome)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Nome", "%" + nome + "%"))
                .Add(Restrictions.Like("NomeFantasia", "%" + nome + "%")));

            criteria.AddOrder(Order.Asc("Nome"));

            return dao.Listar(criteria);
        }

        public virtual void DisposeDAO()
        {
            dao.Dispose();
        }
    }
}
