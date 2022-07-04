using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOLoja : DAO<Loja>
    {
        public DAOLoja(ISession _session) : base(_session) { }

        public async override Task<IList<Loja>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar loja");
                throw new Exception($"Erro ao listar lojas. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Loja>> ListarMatrizes()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.IsNull("Matriz"));
                criteria.Add(Restrictions.Not(Restrictions.Eq("Cnpj", "000000000")));
                criteria.Add(Restrictions.Not(Restrictions.Eq("Cnpj", "11111111111111")));
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar matrizes");
                throw new Exception($"Erro ao listar matrizes. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Loja>> ListarPorCnpj(string termo)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Disjunction().Add(Restrictions.Like("Cnpj", "%" + termo + "%")));
                criteria.AddOrder(Order.Asc("Cnpj"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar lojas por cnpj");
                throw new Exception($"Erro ao listar lojas por cnpj. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Loja>> ListarSomenteLojas()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Not(Restrictions.Eq("Cnpj", "000000000")));
                criteria.Add(Restrictions.Not(Restrictions.Eq("Cnpj", "11111111111111")));
                criteria.AddOrder(Order.Asc("Cnpj"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar somente lojas");
                throw new Exception($"Erro ao listar somente lojas. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Loja>> ListarExcetoDeposito()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Not(Restrictions.Eq("Cnpj", "000000000")));
                criteria.AddOrder(Order.Asc("Cnpj"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar lojas exceto deposito");
                throw new Exception($"Erro ao listar lojas com exceção de depósito. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Loja>> ListarPorNome(string termo)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Disjunction().Add(Restrictions.Like("Nome", "%" + termo + "%")));
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar lojas por nome");
                throw new Exception($"Erro ao listar lojas por nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
