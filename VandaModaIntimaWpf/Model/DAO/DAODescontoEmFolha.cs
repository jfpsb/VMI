using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAODescontoEmFolha : DAO<Model.DescontoEmFolha>
    {
        public DAODescontoEmFolha(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Lista todos os descontos em folha que ocorrem todo mês.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<DescontoEmFolha>> ListarDescontosEmFolhaMensais(Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("RepeteMensal", true));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de descontos em folha por funcionario");
                throw new Exception($"Erro ao listar descontos em folha por funcionario. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        /// <summary>
        /// Lista os descontos da folha atual.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<DescontoEmFolha>> ListarDescontosEmFolhaAtual(Funcionario funcionario, int mes, int ano)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("MesReferencia", mes));
                criteria.Add(Restrictions.Eq("AnoReferencia", ano));
                criteria.Add(Restrictions.Eq("RepeteMensal", false));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de descontos em folha por funcionario e folha");
                throw new Exception($"Erro ao listar descontos em folha por funcionario e folha. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
