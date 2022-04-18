using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOParcela : DAO<Parcela>
    {
        public DAOParcela(ISession session) : base(session)
        {
        }

        public async Task<IList<Parcela>> ListarPorFuncionarioMesAno(Funcionario funcionario, int mes, int ano)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Adiantamento", "Adiantamento");
                criteria.Add(Restrictions.Eq("Adiantamento.Funcionario", funcionario))
                    .Add(Restrictions.Eq("Mes", mes))
                    .Add(Restrictions.Eq("Ano", ano));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar parcelas por funcionario");
                throw new Exception($"Erro ao listar parcelas por funcionário. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Parcela>> ListarPorFuncionarioNaoPagas(Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Adiantamento", "Adiantamento");
                criteria.Add(Restrictions.Eq("Adiantamento.Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Paga", false));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar parcelas não pagas");
                throw new Exception($"Erro ao listar parcelas não pagas. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Parcela>> ListarPorFuncionarioNaoPagasExcetoMesAno(Funcionario funcionario, int mes, int ano)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Adiantamento", "Adiantamento");
                criteria.Add(Restrictions.Eq("Adiantamento.Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Paga", false));
                criteria.Add(Restrictions.Disjunction().
                    Add(Restrictions.Not(Restrictions.Eq("Mes", mes))).
                    Add(Restrictions.Not(Restrictions.Eq("Ano", ano))));
                return await Listar(criteria);
            }
            catch(Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar parcelas nao pagas exceto");
                throw new Exception($"Erro ao listar parcelas não pagas. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
