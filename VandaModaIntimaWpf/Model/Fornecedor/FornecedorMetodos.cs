using NHibernate;
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

        public virtual bool Atualizar()
        {
            return dao.Atualizar(this);
        }

        public virtual bool Deletar()
        {
            return dao.Deletar(this);
        }

        public virtual bool Deletar(IList<Fornecedor> objetos)
        {
            return dao.Deletar(objetos);
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
        public virtual IList<Fornecedor> ListarPorCnpj(string cnpj)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cnpj", "%" + cnpj + "%")));

            criteria.AddOrder(Order.Asc("Cnpj"));

            return dao.Listar(criteria);
        }
        public virtual IList<Fornecedor> ListarPorEmail(string email)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Email", "%" + email + "%")));

            criteria.AddOrder(Order.Asc("Email"));

            return dao.Listar(criteria);
        }

        public virtual IList<Fornecedor> Listar()
        {
            return dao.Listar(dao.CriarCriteria());
        }

        public virtual Fornecedor ListarPorId(string id)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Like("Cnpj", id));

            var result = dao.Listar(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }

        public virtual string[] GetColunas()
        {
            return new string[] { "CNPJ", "Nome", "Nome Fantasia", "Email" };
        }
    }
}
