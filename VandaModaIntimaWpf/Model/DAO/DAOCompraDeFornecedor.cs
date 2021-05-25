using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOCompraDeFornecedor : DAO<CompraDeFornecedor>
    {
        public DAOCompraDeFornecedor(ISession session) : base(session)
        {
        }

        public async Task<IList<CompraDeFornecedor>> ListarPorRepresentante(string nomeRepresentante, DateTime data)
        {
            var criteria = CriarCriteria();
            criteria.CreateAlias("Fornecedor", "Fornecedor");
            criteria.CreateAlias("Fornecedor.Representante", "Representante");
            criteria.Add(Restrictions.Like("Representante.Nome", $"%{nomeRepresentante}%"));
            criteria.Add(Expression.Sql($"YEAR(DataPedido) = ?", data.Year, NHibernateUtil.Int32));
            criteria.Add(Expression.Sql($"MONTH(DataPedido) = ?", data.Month, NHibernateUtil.Int32));
            return await Listar(criteria);
        }

        public async Task<IList<CompraDeFornecedor>> ListarPorLoja(Loja loja, DateTime data)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("Loja", loja));
            criteria.Add(Expression.Sql($"YEAR(DataPedido) = ?", data.Year, NHibernateUtil.Int32));
            criteria.Add(Expression.Sql($"MONTH(DataPedido) = ?", data.Month, NHibernateUtil.Int32));
            return await Listar(criteria);
        }

        public async Task<IList<CompraDeFornecedor>> ListarPorFornecedor(string nomeFornecedor, DateTime data)
        {
            var criteria = CriarCriteria();
            criteria.CreateAlias("Fornecedor", "Fornecedor");
            criteria.Add(Restrictions.Like("Fornecedor.Nome", $"%{nomeFornecedor}%"));
            criteria.Add(Expression.Sql($"YEAR(DataPedido) = ?", data.Year, NHibernateUtil.Int32));
            criteria.Add(Expression.Sql($"MONTH(DataPedido) = ?", data.Month, NHibernateUtil.Int32));
            return await Listar(criteria);
        }
    }
}
