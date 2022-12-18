using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOParcelaCartao : DAO<ParcelaCartao>
    {
        public DAOParcelaCartao(ISession session) : base(session)
        {
        }

        public async Task<IList<ParcelaCartao>> ListarPorMesLojas(DateTime mesReferencia, params Loja[] lojas)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("VendaEmCartao", "VendaEmCartao");

                var disjuncao = Restrictions.Disjunction();
                foreach (var loja in lojas)
                {
                    disjuncao.Add(Restrictions.Eq("VendaEmCartao.Loja", loja));
                }

                criteria.Add(Expression.Sql("YEAR({alias}.data_pagamento) = ?", mesReferencia.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.data_pagamento) = ?", mesReferencia.Month, NHibernateUtil.Int32));
                criteria.Add(disjuncao);

                criteria.AddOrder(Order.Asc("DataPagamento"));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar parcela cartao por mes loja");
                throw new Exception($"Erro ao retornar parcelas de cartão por mês e lojas. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<ParcelaCartao>> ListarPorMesLojasGroupByDiaOperadora(DateTime mesReferencia,
            bool agruparPorDia,
            bool agruparPorOperadora,
            params Loja[] lojas)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("VendaEmCartao", "VendaEmCartao");

                var disjuncao = Restrictions.Disjunction();
                foreach (var loja in lojas)
                {
                    disjuncao.Add(Restrictions.Eq("VendaEmCartao.Loja", loja));
                }

                criteria.Add(Expression.Sql("YEAR({alias}.data_pagamento) = ?", mesReferencia.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.data_pagamento) = ?", mesReferencia.Month, NHibernateUtil.Int32));
                criteria.Add(disjuncao);

                var projecoes = Projections.ProjectionList();

                projecoes.Add(Projections.Sum("ValorBruto"), "ValorBruto");
                projecoes.Add(Projections.Sum("ValorLiquido"), "ValorLiquido");

                if (agruparPorDia)
                {
                    projecoes.Add(Projections.GroupProperty("DataPagamento"), "DataPagamento");
                }

                if (agruparPorOperadora)
                {
                    projecoes.Add(Projections.Property("VendaEmCartao"), "VendaEmCartao");
                    projecoes.Add(Projections.GroupProperty("VendaEmCartao.OperadoraCartao"));
                }

                criteria.SetProjection(projecoes);
                criteria.SetResultTransformer(Transformers.AliasToBean<ParcelaCartao>());
                criteria.AddOrder(Order.Asc("DataPagamento"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar parcela cartao por mes loja");
                throw new Exception($"Erro ao retornar parcelas de cartão por mês e lojas. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
