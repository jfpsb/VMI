using NHibernate.Criterion;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.Model
{
    class MarcaModel
    {
        private IDAO<Marca> dao;
        public Marca marca;

        public MarcaModel()
        {
            dao = new DAOMySQL<Marca>();
        }

        public virtual bool Salvar()
        {
            return dao.Inserir(marca);
        }

        public virtual IList<Marca> Listar()
        {
            var criteria = dao.CriarCriteria();
            return dao.Listar(criteria);
        }

        public virtual IList<Marca> ListarPorNome(string nome)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
            criteria.AddOrder(Order.Asc("Nome"));

            return dao.Listar(criteria);
        }

        public virtual void DisposeDAO()
        {
            dao.Dispose();
        }
    }
}
