using NHibernate.Criterion;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.Model.Fornecedor
{
    public partial class Fornecedor : ObservableObject, Model<Fornecedor>
    {
        private IDAO<Fornecedor> dao;

        public Fornecedor()
        {
            dao = new DAOMySQL<Fornecedor>();
        }

        public virtual bool Salvar()
        {
            return dao.Inserir(this);
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

        public virtual IList<Fornecedor> Listar()
        {
            return dao.Listar(dao.CriarCriteria());
        }

        public virtual void Dispose()
        {
            dao.Dispose();
        }

        public virtual Fornecedor ListarPorId(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
