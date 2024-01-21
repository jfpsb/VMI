using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOCompraDeFornecedor : DAO<CompraDeFornecedor>
    {
        public DAOCompraDeFornecedor(ISession session) : base(session)
        {
        }
        public async Task<IList<CompraDeFornecedor>> ListarPorRepresentante(string nomeRepresentante, DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Fornecedor", "Fornecedor");
                criteria.CreateAlias("Fornecedor.Representante", "Representante");
                criteria.Add(Restrictions.Like("Representante.Nome", $"%{nomeRepresentante}%"));
                criteria.Add(Expression.Sql($"YEAR(DataPedido) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql($"MONTH(DataPedido) = ?", data.Month, NHibernateUtil.Int32));
                criteria.AddOrder(Order.Asc("DataPedido"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de compra de fornecedor por representante em DAOCompraDeFornecedor");
                throw new Exception($"Erro ao listar compras de fornecedor por representante. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<CompraDeFornecedor>> ListarPorLoja(Loja loja, DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Loja", loja));
                criteria.Add(Expression.Sql($"YEAR(DataPedido) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql($"MONTH(DataPedido) = ?", data.Month, NHibernateUtil.Int32));
                criteria.AddOrder(Order.Asc("DataPedido"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de compra de fornecedor por loja em DAOCompraDeFornecedor");
                throw new Exception($"Erro ao listar compras de fornecedor por loja. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<CompraDeFornecedor>> ListarPorFornecedor(string nomeFornecedor, DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Fornecedor", "Fornecedor");
                criteria.Add(Restrictions.Like("Fornecedor.Nome", $"%{nomeFornecedor}%"));
                criteria.Add(Expression.Sql($"YEAR(DataPedido) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql($"MONTH(DataPedido) = ?", data.Month, NHibernateUtil.Int32));
                criteria.AddOrder(Order.Asc("DataPedido"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de compra de fornecedor por fornecedor em DAOCompraDeFornecedor");
                throw new Exception($"Erro ao listar compras de fornecedor por fornecedor. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<CompraDeFornecedor>> ListarPorData(DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Expression.Sql($"YEAR(DataPedido) = ?", data.Year, NHibernateUtil.Int32));
                criteria.Add(Expression.Sql($"MONTH(DataPedido) = ?", data.Month, NHibernateUtil.Int32));
                criteria.AddOrder(Order.Asc("DataPedido"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de compra de fornecedor por data em DAOCompraDeFornecedor");
                throw new Exception($"Erro ao listar compras de fornecedor por data. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<CompraDeFornecedor>> ListarComprasAPagar()
        {
            try
            {
                var criteria = CriarCriteria("CompraFornecedor");

                var c2 = criteria.CreateCriteria("Loja", "Loja", NHibernate.SqlCommand.JoinType.RightOuterJoin, Restrictions.Conjunction()
                    .Add(Restrictions.Eq("CompraFornecedor.Deletado", false))
                    .Add(Restrictions.Eq("CompraFornecedor.Pago", false)))
                    .Add(Restrictions.Conjunction()
                    .Add(Restrictions.Not(Restrictions.Eq("Cnpj", "000000000")))
                    .Add(Restrictions.Not(Restrictions.Eq("Cnpj", "11111111111111")))
                    .Add(Restrictions.Eq("Deletado", false)));

                criteria.AddOrder(Order.Asc("Loja.Nome"));
                criteria.SetProjection(
                    Projections.RootEntity(),
                    Projections.ProjectionList()
                    .Add(Projections.Property("Loja"), "Loja")
                    .Add(Projections.Sum("Valor"), "Valor")
                    .Add(Projections.GroupProperty("Loja.Cnpj")));

                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);

                var result = await criteria.ListAsync<object[]>();
                IList<CompraDeFornecedor> compras = new List<CompraDeFornecedor>();

                foreach (var item in result)
                {
                    CompraDeFornecedor compra = new CompraDeFornecedor();
                    var loja = await session.GetAsync<Loja>(item[3]);
                    compra.Loja = loja;

                    if (item[0] == null)
                    {
                        compra.Valor = 0;
                    }
                    else
                    {
                        compra.Valor = (double)item[2];
                    }

                    compras.Add(compra);
                }

                return compras;
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de compras a pagar");
                throw new Exception($"Erro ao listar compras a pagar. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
