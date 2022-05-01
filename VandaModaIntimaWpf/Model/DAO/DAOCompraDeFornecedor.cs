using NHibernate;
using NHibernate.Criterion;
using System;
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
    }
}
