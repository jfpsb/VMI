using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFaltas : DAO<Faltas>
    {
        public DAOFaltas(ISession session) : base(session)
        {
        }

        public async Task<Faltas> ListarFaltasPorMesFuncionarioSoma(int ano, int mes, Funcionario funcionario)
        {
            try
            {
                int daysInMonth = DateTime.DaysInMonth(ano, mes);

                var criteria = CriarCriteria();

                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Deletado", false));
                criteria.Add(Restrictions.Eq("Justificado", false));
                criteria.Add(Restrictions.Between("Data", new DateTime(ano, mes, 1, 0, 0, 0), new DateTime(ano, mes, daysInMonth, 23, 59, 59)));
                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("Horas"), "Horas")
                    .Add(Projections.Sum("Minutos"), "Minutos")
                    .Add(Projections.Property("Data"), "Data")
                    .Add(Projections.Property("Id"), "Id")
                    .Add(Projections.Property("Justificado"), "Justificado")
                    .Add(Projections.Property("Uuid"), "Uuid")
                    .Add(Projections.GroupProperty("Funcionario"), "Funcionario"));

                criteria.SetResultTransformer(Transformers.AliasToBean<Faltas>());

                return await criteria.UniqueResultAsync<Faltas>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de faltas por mês e funcionário");
                throw new Exception($"Erro ao listar faltas de funcionário neste mês. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<Faltas> ListarPorDiaFuncionario(DateTime dia, Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Data", dia));
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                return await criteria.UniqueResultAsync<Faltas>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de falta por dia e funcionário");
                throw new Exception($"Erro ao listar falta de funcionário por dia. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
