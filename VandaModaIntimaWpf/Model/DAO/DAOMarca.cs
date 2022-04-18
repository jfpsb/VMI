using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOMarca : DAO<Marca>
    {
        public DAOMarca(ISession session) : base(session) { }

        public async Task<IList<Marca>> ListarPorNome(string nome)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar marcas por nome");
                throw new Exception($"Erro ao listar marcas por nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
