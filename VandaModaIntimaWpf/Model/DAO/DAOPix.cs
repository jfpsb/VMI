using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOPix : DAO<Model.Pix>
    {
        public DAOPix(ISession session) : base(session)
        {
        }

        public async Task<IList<Model.Pix>> ListarPixPorDiaLoja(DateTime dia, Loja loja)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Between("Horario", dia.Date, dia.Date.AddHours(23).AddMinutes(59)));
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
