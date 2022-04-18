using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOBonusMensal : DAO<BonusMensal>
    {
        public DAOBonusMensal(NHibernate.ISession session) : base(session)
        {
        }

        public async Task<IList<BonusMensal>> ListarPorFuncionario(Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de bonusmensal por funcionário em DAOBonusMensal");
                throw new Exception($"Erro ao listar bônus mensais de funcionário. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
