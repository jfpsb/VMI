﻿using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOEntradaMercadoriaProdutoGrade : DAO<Model.EntradaMercadoriaProdutoGrade>
    {
        public DAOEntradaMercadoriaProdutoGrade(ISession session) : base(session)
        {
        }

        public async Task<IList<Model.EntradaMercadoriaProdutoGrade>> ListarPorPeriodoFornecedor(DateTime periodo, Fornecedor fornecedor)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.CreateAlias("ProdutoGrade", "ProdutoGrade", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
                criteria.CreateAlias("Entrada", "Entrada");

                criteria.Add(Expression.Sql("YEAR(Data) = ?", periodo.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH(Data) = ?", periodo.Month, NHibernateUtil.Int32));
                criteria.Add(Restrictions.Eq("GradeFornecedor", fornecedor));
                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("Quantidade"), "Quantidade")
                    .Add(Projections.Property("GradePreco"), "GradePreco")
                    .Add(Projections.GroupProperty("ProdutoGrade"), "ProdutoGrade")
                    .Add(Projections.GroupProperty("ProdutoDescricao"), "ProdutoDescricao")
                    .Add(Projections.GroupProperty("GradeDescricao"), "GradeDescricao"));

                criteria.AddOrder(Order.Asc("ProdutoDescricao"));

                criteria.SetResultTransformer(Transformers.AliasToBean<Model.EntradaMercadoriaProdutoGrade>());

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de entradamercadoriaproduto por mês, ano, fornecedor");
                throw new Exception($"Erro ao listar entradas de mercadoria por data e fornecedor. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
