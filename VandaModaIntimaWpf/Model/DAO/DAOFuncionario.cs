using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFuncionario : DAO<Funcionario>
    {
        public DAOFuncionario(ISession session) : base(session) { }

        public async override Task<IList<Funcionario>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar funcionário");
                throw new Exception($"Erro ao listar funcionários. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Funcionario>> ListarPorNome(string nome)
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
                Log.EscreveLogBanco(ex, "listar funcionário por nome");
                throw new Exception($"Erro ao listar funcionários por nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Funcionario>> ListarPorCpf(string cnpj)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Like("Cpf", "%" + cnpj + "%"));
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar funcionário por cpf");
                throw new Exception($"Erro ao listar funcionários por cpf. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
