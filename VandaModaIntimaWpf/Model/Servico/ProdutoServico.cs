﻿using NHibernate.Criterion;
using NHibernate.Transform;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.Model.Servico
{
    class ProdutoServico
    {
        private IDAO<Produto> dao;

        public ProdutoServico()
        {
            dao = new DAOMySQL<Produto>();
        }

        public virtual bool Salvar(Produto produto)
        {
            bool result = dao.Inserir(produto);

            if (result)
                return true;

            return false;
        }

        public virtual IList<Produto> Listar()
        {
            var criteria = dao.CriarCriteria();
            return dao.Listar(criteria);
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

        public virtual void DisposeDAO()
        {
            dao.Dispose();
        }
    }
}
