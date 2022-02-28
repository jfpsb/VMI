using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOEntradaMercadoriaProdutoGrade : DAO<Model.EntradaMercadoriaProdutoGrade>
    {
        public DAOEntradaMercadoriaProdutoGrade(ISession session) : base(session)
        {
        }

        public async Task<IList<Model.EntradaMercadoriaProdutoGrade>> ListarPorPeriodoFornecedor(DateTime periodo, Fornecedor fornecedor)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("ProdutoGrade", "ProdutoGrade");
            criteria.CreateAlias("ProdutoGrade.Produto", "Produto");
            criteria.CreateAlias("Entrada", "Entrada");

            criteria.Add(Expression.Sql("YEAR(Data) = ?", periodo.Year, NHibernateUtil.Int32));
            criteria.Add(Expression.Sql("MONTH(Data) = ?", periodo.Month, NHibernateUtil.Int32));
            criteria.Add(Restrictions.Eq("Produto.Fornecedor", fornecedor));
            //criteria.Add(Restrictions.Eq("ProdutoGrade.Produto.Fornecedor", fornecedor));
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Sum("Quantidade"), "Quantidade")
                .Add(Projections.GroupProperty("ProdutoGrade"), "ProdutoGrade"));

            criteria.AddOrder(Order.Asc("Produto.Descricao"));

            criteria.SetResultTransformer(Transformers.AliasToBean<Model.EntradaMercadoriaProdutoGrade>());

            return await Listar(criteria);
        }
    }
}
