using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAODespesa : DAO<Model.Despesa>
    {
        public DAODespesa(ISession session) : base(session)
        {
        }

        public async override Task<IList<Despesa>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Descricao"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de despesa em DAODespesa");
                throw new Exception($"Erro ao listar contagens por loja. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<double> RetornaSomaTodasDespesas(DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.Add(Expression.Sql("YEAR({alias}.Data) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.Data) = ?", data.Month, NHibernateUtil.Int32));
                criteria.Add(Restrictions.Eq("Deletado", false));
                criteria.SetProjection(Projections.Sum("Valor"));
                var result = await criteria.UniqueResultAsync();

                if (result != null)
                    return (double)result;

                return 0;
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "soma de despesas em DAODespesa");
                throw new Exception($"Erro ao retornar soma dos valores de despesas. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Despesa>> ListarPorTipoDespesaFiltroMesAno(TipoDespesa tipoDespesa, Loja loja, DateTime data, string filtro, string termo)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.Add(Restrictions.Eq("TipoDespesa", tipoDespesa));

                switch (filtro)
                {
                    case "Fornecedor":
                        criteria.CreateAlias("Fornecedor", "Fornecedor");
                        AdicionaTermosPesquisa(criteria, "Fornecedor.Nome", termo.Split(new[] { " OU ".ToLower() }, StringSplitOptions.None));
                        break;
                    case "Descrição":
                        AdicionaTermosPesquisa(criteria, "Descricao", termo.Split(new[] { " OU ".ToLower() }, StringSplitOptions.None));
                        break;
                    case "Membro Familiar":
                        criteria.CreateAlias("Familiar", "Familiar");
                        AdicionaTermosPesquisa(criteria, "Familiar.Nome", termo.Split(new[] { " OU ".ToLower() }, StringSplitOptions.None));
                        break;
                }

                if (loja.Cnpj != null)
                {
                    criteria.Add(Restrictions.Eq("Loja", loja));
                }

                criteria.Add(Expression.Sql("YEAR({alias}.Data) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.Data) = ?", data.Month, NHibernateUtil.Int32));
                criteria.AddOrder(Order.Asc("Data"));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de despesa por tipodespesa");
                throw new Exception($"Erro ao listar despesas por tipo de despesa. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Despesa>> ListarPorMesAno(DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Expression.Sql("YEAR({alias}.Data) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql("MONTH({alias}.Data) = ?", data.Month, NHibernateUtil.Int32));
                criteria.AddOrder(Order.Asc("Data"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de despesa por mês e ano");
                throw new Exception($"Erro ao listar contagens por mês e ano. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        private void AdicionaTermosPesquisa(ICriteria criteria, string propertyName, string[] termos)
        {
            var disjunction = Restrictions.Disjunction();

            foreach (string termo in termos)
            {
                disjunction.Add(Restrictions.Like(propertyName, $"%{termo}%"));
            }

            criteria.Add(disjunction);
        }
    }
}
