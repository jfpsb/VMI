using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO.Pix
{
    public class DAOPix : DAO<Model.Pix.Pix>
    {
        public DAOPix(ISession session) : base(session)
        {
        }

        public async Task<IList<Model.Pix.Pix>> ListarPixPorDiaLoja(DateTime dia, Loja loja)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Between("Horario", dia.Date.ToUniversalTime(), dia.Date.ToUniversalTime().AddHours(23).AddMinutes(59)));
                criteria.Add(Restrictions.Eq("Loja", loja));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar pix por dia, loja");
                throw new Exception($"Erro ao listar pix por dia e ljoa. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
