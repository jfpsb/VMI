using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOHoraExtra : DAO<HoraExtra>
    {
        public DAOHoraExtra(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Lista horas extras de mês e funcionário específico.
        /// </summary>
        /// <param name="ano">Ano para consulta.</param>
        /// <param name="mes">Mês para consulta.</param>
        /// <param name="funcionario">Funcionário para consulta.</param>
        /// <returns>Lista com horas extras encontradas ou nenhum item.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IList<HoraExtra>> ListarPorMesFuncionario(int ano, int mes, Funcionario funcionario)
        {
            try
            {
                int daysInMonth = DateTime.DaysInMonth(ano, mes);
                var criteria = CriarCriteria();
                criteria
                    .Add(Restrictions.Between("Data", new DateTime(ano, mes, 1, 0, 0, 0), new DateTime(ano, mes, daysInMonth, 23, 59, 59)))
                    .Add(Restrictions.Eq("Funcionario", funcionario));

                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("Horas"), "Horas")
                    .Add(Projections.Sum("Minutos"), "Minutos")
                    .Add(Projections.Property("Data"), "Data")
                    .Add(Projections.Property("Id"), "Id")
                    .Add(Projections.Property("LojaTrabalho"), "LojaTrabalho")
                    .Add(Projections.Property("Uuid"), "Uuid")
                    .Add(Projections.GroupProperty("Funcionario"), "Funcionario")
                    .Add(Projections.GroupProperty("TipoHoraExtra"), "TipoHoraExtra"));

                criteria.SetResultTransformer(Transformers.AliasToBean<HoraExtra>());

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar hora extra data funcionario");
                throw new Exception($"Erro ao listar horas extras. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        /// <summary>
        /// Lista horas extras de mês, funcionário e tipo de hora extra específicos.
        /// </summary>
        /// <param name="dia">Dia para consulta</param>
        /// <param name="funcionario">Funcionário para consulta</param>
        /// <param name="tipo">Tipo de hora extra para consulta</param>
        /// <returns>Hora extra específica do dia ou null</returns>
        /// <exception cref="Exception"></exception>
        public async Task<HoraExtra> ListarPorDiaFuncionarioTipoHoraExtra(DateTime dia, Funcionario funcionario, TipoHoraExtra tipo)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Data", dia))
                    .Add(Restrictions.Eq("Funcionario", funcionario))
                    .Add(Restrictions.Eq("TipoHoraExtra", tipo));
                return await criteria.UniqueResultAsync<HoraExtra>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar hora extra por dia funcionario");
                throw new Exception($"Erro ao listar hora extra. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        /// <summary>
        /// Lista horas extras de mês e funcionário específicos, agrupados por tipo de hora extra.
        /// </summary>
        /// <param name="data">Datetime com mês e ano para consulta.</param>
        /// <param name="funcionario">Funcionário para consulta</param>
        /// <returns>Lista de horas extras ou nenhum item</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IList<HoraExtra>> ListarPorMesFuncionarioGroupByTipoHoraExtra(DateTime data, Funcionario funcionario)
        {
            try
            {
                DateTime inicio = new DateTime(data.Year, data.Month, 1);
                DateTime fim = inicio.AddMonths(1).AddSeconds(-1);
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Between("Data", inicio, fim))
                    .Add(Restrictions.Eq("Funcionario", funcionario));

                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("Horas"), "Horas")
                    .Add(Projections.Sum("Minutos"), "Minutos")
                    .Add(Projections.Property("Data"), "Data")
                    .Add(Projections.Property("Id"), "Id")
                    .Add(Projections.Property("LojaTrabalho"), "LojaTrabalho")
                    .Add(Projections.Property("Uuid"), "Uuid")
                    .Add(Projections.Property("Funcionario"), "Funcionario")
                    .Add(Projections.GroupProperty("TipoHoraExtra"), "TipoHoraExtra"));

                criteria.SetResultTransformer(Transformers.AliasToBean<HoraExtra>());
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar hora extra por dia funcionario agrupado por tipo de hora");
                throw new Exception($"Erro ao listar hora extra por período, agrupado por tipo de hora extra. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<HoraExtra> ListarPorDiaFuncionario(DateTime dia, Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Data", dia))
                    .Add(Restrictions.Eq("Funcionario", funcionario));
                return await criteria.UniqueResultAsync<HoraExtra>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar hora extra por dia funcionario");
                throw new Exception($"Erro ao listar hora extra. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<HoraExtra> ListarSomaPorMesFuncionarioTipoHoraExtra(int mes, int ano, Funcionario funcionario, TipoHoraExtra tipo)
        {
            try
            {
                DateTime inicio = new DateTime(ano, mes, 1);
                DateTime fim = inicio.AddMonths(1).AddSeconds(-1);
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Between("Data", inicio, fim))
                    .Add(Restrictions.Eq("Funcionario", funcionario))
                    .Add(Restrictions.Eq("TipoHoraExtra", tipo));

                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("Horas"), "Horas")
                    .Add(Projections.Sum("Minutos"), "Minutos"));

                criteria.SetResultTransformer(Transformers.AliasToBean<HoraExtra>());

                return await criteria.UniqueResultAsync<HoraExtra>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar soma hora extra por mes funcionario por tipo de hora");
                throw new Exception($"Erro ao listar hora extra por mês de funcionário com tipo específico. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
