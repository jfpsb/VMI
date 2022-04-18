using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOTipoContagem : DAO<TipoContagem>
    {
        public DAOTipoContagem(ISession session) : base(session) { }

        public async Task<IList<TipoContagem>> ListarPorNome(string nome)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Nome", nome));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar tipo contagem");
                throw new Exception($"Erro ao listar tipos de contagem. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
