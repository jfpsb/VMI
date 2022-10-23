using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFaltas : DAO<Faltas>
    {
        public DAOFaltas(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Soma o total de horas e minutos de todas as faltas de funcionário em determinado mês, exceto faltas justificadas.
        /// </summary>
        /// <param name="ano">Ano para consulta</param>
        /// <param name="mes">Mês para consulta</param>
        /// <param name="funcionario">Funcionário para consultar faltas</param>
        /// <returns>Objeto Falta com a soma das horas e minutos ou nulo.</returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// Lista todas as faltas de funcionário em determinado mês, incluindo justificadas ou não.
        /// </summary>
        /// <param name="ano">Ano para consulta</param>
        /// <param name="mes">Mês para consulta</param>
        /// <param name="funcionario">Funcionário para consulta</param>
        /// <returns>Lista com todas as faltas ou nenhum item</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IList<Faltas>> ListarFaltasPorMesFuncionario(int ano, int mes, Funcionario funcionario)
        {
            try
            {
                var inicio = new DateTime(ano, mes, 1);
                var fim = inicio.AddMonths(1).AddSeconds(-1);

                var criteria = CriarCriteria();

                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Deletado", false));
                criteria.Add(Restrictions.Between("Data", inicio, fim));
                criteria.AddOrder(Order.Asc("Data"));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de faltas por mês e funcionário");
                throw new Exception($"Erro ao listar faltas de funcionário neste mês. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        /// <summary>
        /// Lista falta não justificada de funcionário em dia específico.
        /// </summary>
        /// <param name="dia">Datetime contendo o dia para consulta.</param>
        /// <param name="funcionario">Funcionário para consultar faltas</param>
        /// <returns>Falta encontrada em banco de dados ou null</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Faltas> ListarPorDiaFuncionario(DateTime dia, Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Data", dia));
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Justificado", false));
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
