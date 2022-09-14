using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.Pix;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO.Pix
{
    public class DAOCobranca : DAO<Model.Pix.Cobranca>
    {
        public DAOCobranca(ISession session) : base(session)
        {
        }

        public async Task<IList<Cobranca>> ListarPorDiaELoja(DateTime dia, Model.Loja loja)
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {

                    var criteria = CriarCriteria();
                    criteria.CreateAlias("Calendario", "Calendario");
                    criteria.AddOrder(Order.Asc("Calendario.Criacao"));
                    criteria.Add(Restrictions.Eq("Loja", loja));
                    criteria.Add(Restrictions.Between("Calendario.Criacao", dia.Date.ToUniversalTime(), dia.Date.ToUniversalTime().AddDays(1).AddSeconds(-1)));
                    return await Listar(criteria);

                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Log.EscreveLogBanco(ex, "listar por dia em daocobranca");
                    throw new Exception($"Erro ao listar por dia em daocobranca em banco de dados local. Acesse {Log.LogBanco} para mais detalhes.", ex);
                }
            }
        }
    }
}
