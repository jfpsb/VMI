using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOTipoGrade : DAO<TipoGrade>
    {
        public DAOTipoGrade(ISession session) : base(session) { }

        public async override Task<IList<TipoGrade>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar tipo de grade");
                throw new Exception($"Erro ao listar tipos de grade. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<TipoGrade> ListarPorNome(string nome)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Like("Nome", nome));
                return await criteria.UniqueResultAsync<TipoGrade>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar tipo de grade por nome");
                throw new Exception($"Erro ao listar tipos de grade por nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
