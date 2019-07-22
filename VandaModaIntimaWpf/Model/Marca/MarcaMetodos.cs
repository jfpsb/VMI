using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.Model.Marca
{
    public partial class Marca : ObservableObject, Model<Marca>
    {
        private IDAO<Marca> dao;
        public Marca()
        {
            dao = new DAOMySQL<Marca>();
        }

        public virtual bool Salvar()
        {
            return dao.Inserir(this);
        }
        public virtual bool Atualizar()
        {
            return dao.Atualizar(this);
        }
        public virtual IList<Marca> ListarPorNome(string nome)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
            criteria.AddOrder(Order.Asc("Nome"));

            return dao.Listar(criteria);
        }

        public virtual IList<Marca> Listar()
        {
            return dao.Listar(dao.CriarCriteria());
        }

        public virtual Marca ListarPorId(string id)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Like("Id", id));

            var result = dao.Listar(criteria);

            if (result.Count == 0)
                return null;

            return result[0];
        }

        public virtual bool Deletar()
        {
            throw new System.NotImplementedException();
        }
    }
}
