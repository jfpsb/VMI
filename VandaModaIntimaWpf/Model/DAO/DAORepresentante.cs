using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAORepresentante : DAO<Representante>
    {
        public DAORepresentante(ISession session) : base(session)
        {
        }

        public async override Task<IList<Representante>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar representante");
                throw new Exception($"Erro ao listar representantes. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Representante>> ListarPorNome(string nome)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Like("Nome", $"%{nome}%"));
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar representante por nome");
                throw new Exception($"Erro ao listar representantes por nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
