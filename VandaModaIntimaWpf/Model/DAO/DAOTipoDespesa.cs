using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOTipoDespesa : DAO<TipoDespesa>
    {
        public DAOTipoDespesa(ISession session) : base(session)
        {
        }

        public async override Task<IList<TipoDespesa>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar tipo despesa");
                throw new Exception($"Erro ao listar tipos de despesa. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<TipoDespesa> RetornaTipoDespesaEmpresarial()
        {
            return await ListarPorId("c75ac132-c365-11ec-be0f-b482559537dc");
        }
    }
}
