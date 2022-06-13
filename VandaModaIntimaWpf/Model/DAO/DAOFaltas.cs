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

        public async Task<Faltas> GetFaltasPorMesFuncionario(int ano, int mes, Funcionario funcionario)
        {
            try
            {
                int daysInMonth = DateTime.DaysInMonth(ano, mes);

                var criteria = CriarCriteria();

                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Between("Data", new DateTime(ano, mes, 1, 0, 0, 0), new DateTime(ano, mes, daysInMonth, 23, 59, 59)));
                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("Horas"), "Horas")
                    .Add(Projections.Sum("Minutos"), "Minutos")
                    .Add(Projections.Property("Data"), "Data")
                    .Add(Projections.Property("Id"), "Id")
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
    }
}
