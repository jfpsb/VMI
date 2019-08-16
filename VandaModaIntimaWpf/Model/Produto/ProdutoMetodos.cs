using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.Model.Produto
{
    public partial class Produto : ObservableObject, IModel<Produto>
    {
        private IDAO<Produto> dao;
        public Produto()
        {
            dao = new DAOMySQL<Produto>();
        }

        public virtual bool Salvar()
        {
            return dao.Inserir(this);
        }
        public virtual bool Salvar(IList<Produto> lista)
        {
            return dao.Inserir(lista);
        }
        public virtual bool Atualizar()
        {
            return dao.Atualizar(this);
        }

        public virtual bool Deletar()
        {
            return dao.Deletar(this);
        }
        public virtual bool Deletar(IList<Produto> objetos)
        {
            return dao.Deletar(objetos);
        }
        public virtual Produto ListarPorId(string id)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Like("Cod_Barra", id));

            var result = dao.Listar(criteria);

            if (result.Count == 0)
                return null;

            return result[0];
        }

        public virtual IList<Produto> ListarPorDescricao(string descricao)
        {
            var criteria = dao.CriarCriteria();

            criteria.Add(Restrictions.Like("Descricao", "%" + descricao + "%"));
            criteria.AddOrder(Order.Asc("Descricao"));

            return dao.Listar(criteria);
        }

        public virtual IList<Produto> ListarPorCodigoDeBarra(string codigo)
        {
            var criteria = dao.CriarCriteria();

            criteria.CreateAlias("Codigos", "CodBarraFornecedor", NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Cod_Barra", "%" + codigo + "%"))
                //Tem que usar 'elements' porque no mapa de Produto os códigos estão mapeados como element
                .Add(Restrictions.Like("CodBarraFornecedor.elements", "%" + codigo + "%")));

            //Por causa do groupby eu tenho que especificar as propriedades que quero recuperar no select
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Property("Cod_Barra"), "Cod_Barra")
                .Add(Projections.Property("Descricao"), "Descricao")
                .Add(Projections.Property("Preco"), "Preco")
                .Add(Projections.Property("Fornecedor"), "Fornecedor")
                .Add(Projections.Property("Marca"), "Marca")
                .Add(Projections.GroupProperty("Cod_Barra")));

            criteria.AddOrder(Order.Asc("Cod_Barra"));

            criteria.SetResultTransformer(Transformers.AliasToBean<Produto>());

            return dao.Listar(criteria);
        }

        public virtual IList<Produto> ListarPorFornecedor(string fornecedor)
        {
            var criteria = dao.CriarCriteria();

            criteria.CreateAlias("Fornecedor", "Fornecedor");

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Fornecedor.Nome", "%" + fornecedor + "%"))
                .Add(Restrictions.Like("Fornecedor.NomeFantasia", "%" + fornecedor + "%")));

            return dao.Listar(criteria);
        }

        public virtual IList<Produto> ListarPorMarca(string marca)
        {
            var criteria = dao.CriarCriteria();

            criteria.CreateAlias("Marca", "Marca");
            criteria.Add(Restrictions.Like("Marca.Nome", "%" + marca + "%"));

            return dao.Listar(criteria);
        }

        public virtual IList<Produto> Listar()
        {
            return dao.Listar(dao.CriarCriteria());
        }

        public virtual string[] GetColunas()
        {
            return new string[] { "Cód. de Barras", "Descrição", "Preço", "Fornecedor", "Marca" };
        }
    }
}
