using NHibernate;
using NHibernate.Criterion;
using System;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOPontoEletronico : DAO<PontoEletronico>
    {
        public DAOPontoEletronico(ISession session) : base(session)
        {
        }

        public async Task<PontoEletronico> ListarPorDiaFuncionario(DateTime dia, Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Dia", dia));
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                return await criteria.UniqueResultAsync<PontoEletronico>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar por dia funcionario em DAOPontoEletronico");
                throw new Exception($"Erro ao listar pontos por dia e funcionário. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
