using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOProvisionamento : DAO<Provisionamento>
    {
        public DAOProvisionamento(ISession session) : base(session) {}

        public async Task<IList<Provisionamento>> ListarProvisionamentosPorMesAno(int mes, int ano)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.CreateAlias("Funcionario", "Funcionario");
                criteria.Add(Restrictions.Eq("Mes", mes));
                criteria.Add(Restrictions.Eq("Ano", ano));
                criteria.Add(Restrictions.IsNull("Funcionario.Demissao"));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "retorna provisionamentos por mes ano funcionario");
                throw new Exception($"Erro ao retornar provisionamentos usando mês, ano. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Provisionamento>> ListarProvisionamentosTotaisGroupByFuncionario()
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.CreateAlias("Funcionario", "Funcionario");
                criteria.Add(Restrictions.IsNull("Funcionario.Demissao"));

                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("AvisoPrevio"), "AvisoPrevio")
                    .Add(Projections.Sum("DecimoTerceiro"), "DecimoTerceiro")
                    .Add(Projections.Sum("MultaFgts"), "MultaFgts")
                    .Add(Projections.GroupProperty("Funcionario"), "Funcionario")
                    );

                criteria.SetResultTransformer(Transformers.AliasToBean<Provisionamento>());

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "retorna provisionamentos totais de cada funcionario não demitido");
                throw new Exception($"Erro ao retornar provisionamentos totais de cada funcionario não demitidos. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
