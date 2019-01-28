using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using VandaModaIntima.Model.DAO;

namespace VandaModaIntima.Model
{
    public class Marca
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }

        public virtual IList<Produto> Produtos { get; set; } = new List<Produto>();

        private IDAO<Marca> daoMarca;

        public Marca()
        {
            daoMarca = new DAOMySQL<Marca>();
        }

        public Marca(long id, string nome)
        {
            Id = id;
            Nome = nome;
            daoMarca = new DAOMySQL<Marca>();
        }

        public virtual bool Salvar(Marca marca)
        {
            bool result = daoMarca.Inserir(marca);

            if (result)
                return true;

            return false;
        }

        public virtual IList<Marca> Listar()
        {
            var criteria = daoMarca.CriarCriteria();
            return daoMarca.Listar(criteria);
        }

        public virtual IList<Marca> ListarPorNome(string nome)
        {
            var criteria = daoMarca.CriarCriteria();

            criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
            criteria.AddOrder(Order.Asc("Nome"));

            return daoMarca.Listar(criteria);
        }

        public virtual void DisposeDAO()
        {
            daoMarca.Dispose();
        }
    }
}
