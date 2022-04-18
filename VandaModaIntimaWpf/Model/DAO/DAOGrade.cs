using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOGrade : DAO<Grade>
    {
        public DAOGrade(ISession session) : base(session) { }
        public async Task<IList<Grade>> ListarPorTipoGrade(TipoGrade tipoGrade)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("TipoGrade", "TipoGrade");
                criteria.Add(Restrictions.Eq("TipoGrade.Id", tipoGrade.Id));
                criteria.AddOrder(Order.Asc("Nome"));
                criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar grade por tipo");
                throw new Exception($"Erro ao listar grade por tipo. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<Grade> ListarPorNome(string nome)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Like("Nome", nome));
                return await criteria.UniqueResultAsync<Grade>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar grade por nome");
                throw new Exception($"Erro ao listar grade por nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
