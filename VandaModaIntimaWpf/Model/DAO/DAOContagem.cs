using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    class DAOContagem : DAO<Contagem>
    {
        public DAOContagem(ISession session) : base(session) { }

        public async Task<IList<Contagem>> ListarPorLojaEPeriodo(Loja loja, DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Loja", loja));
                criteria.Add(Restrictions.Between("Data", dataInicial, dataFinal));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de contagem por loja e data em DAOContagem");
                throw new Exception($"Erro ao listar contagens por loja. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Contagem>> ListarPorTipo(TipoContagem tipoContagem)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("TipoContagem", tipoContagem));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de contagem por tipo");
                throw new Exception($"Erro ao listar contagens por tipo. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
