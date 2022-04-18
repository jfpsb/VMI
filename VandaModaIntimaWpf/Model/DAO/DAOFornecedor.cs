using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOFornecedor : DAO<Fornecedor>
    {
        public DAOFornecedor(ISession session) : base(session) { }

        public async override Task<IList<Fornecedor>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar fornecedor");
                throw new Exception($"Erro ao listar fornecedor. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Fornecedor>> ListarPorNome(string nome)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Disjunction()
                    .Add(Restrictions.Like("Nome", "%" + nome + "%"))
                    .Add(Restrictions.Like("Fantasia", "%" + nome + "%")));
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar fornecedor por nome");
                throw new Exception($"Erro ao listar fornecedor por nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Fornecedor>> ListarPorCnpj(string cnpj)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Disjunction()
                    .Add(Restrictions.Like("Cnpj", "%" + cnpj + "%")));
                criteria.AddOrder(Order.Asc("Cnpj"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar fornecedor por cnpj");
                throw new Exception($"Erro ao listar fornecedor por cnpj. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Fornecedor>> ListarPorEmail(string email)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Disjunction()
                    .Add(Restrictions.Like("Email", "%" + email + "%")));
                criteria.AddOrder(Order.Asc("Email"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar fornecedor por email");
                throw new Exception($"Erro ao listar fornecedor por e-mail. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<Fornecedor> ListarPorIDOuNome(string termo)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Disjunction()
                    .Add(Restrictions.Like("Cnpj", termo))
                    .Add(Restrictions.Like("Nome", termo)));
                var result = await Listar(criteria);
                if (result.Count == 0)
                {
                    return null;
                }
                return result[0];
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar fornecedor por cnpj ou nome");
                throw new Exception($"Erro ao listar fornecedor por cnpj ou nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
