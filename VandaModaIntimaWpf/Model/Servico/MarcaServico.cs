using NHibernate.Criterion;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.Model.Servico
{
    class MarcaServico
    {
        private IDAO<Marca> dao;

        public MarcaServico()
        {
            dao = new DAOMySQL<Marca>();
        }

        public virtual bool Salvar(Marca marca)
        {
            bool result = dao.Inserir(marca);

            if (result)
                return true;

            return false;
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
