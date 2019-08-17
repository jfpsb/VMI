﻿using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOProduto : DAOMySQL<Produto>
    {
        public DAOProduto(ISession session) : base(session) { }

        public override Produto ListarPorId(object id)
        {
            id = (string)id;

            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Cod_Barra", id));

            var result = Listar(criteria);

            if (result.Count == 0)
                return null;

            return result[0];
        }

        public virtual IList<Produto> ListarPorDescricao(string descricao)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Like("Descricao", "%" + descricao + "%"));
            criteria.AddOrder(Order.Asc("Descricao"));

            return Listar(criteria);
        }

        public IList<Produto> ListarPorCodigoDeBarra(string codigo)
        {
            var criteria = CriarCriteria();

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

            return Listar(criteria);
        }

        public IList<Produto> ListarPorFornecedor(string fornecedor)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Fornecedor", "Fornecedor");

            criteria.Add(Restrictions.Disjunction()
                .Add(Restrictions.Like("Fornecedor.Nome", "%" + fornecedor + "%"))
                .Add(Restrictions.Like("Fornecedor.NomeFantasia", "%" + fornecedor + "%")));

            return Listar(criteria);
        }

        public IList<Produto> ListarPorMarca(string marca)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Marca", "Marca");
            criteria.Add(Restrictions.Like("Marca.Nome", "%" + marca + "%"));

            return Listar(criteria);
        }
    }
}