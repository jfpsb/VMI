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
        public DAOProvisionamento(ISession session) : base(session) { }

        public async Task<IList<Provisionamento>> ListarProvisionamentosPorMesAno(int mes, int ano)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.CreateAlias("Funcionario", "Funcionario");

                var mesDisjunction = Restrictions.Disjunction();
                if (mes == 12)
                {
                    mesDisjunction.Add(Restrictions.Eq("Mes", 12));
                    mesDisjunction.Add(Restrictions.Eq("Mes", 13));
                    criteria.Add(mesDisjunction);
                }
                else
                {
                    criteria.Add(Restrictions.Eq("Mes", mes));
                }

                criteria.Add(Restrictions.Eq("Ano", ano));
                criteria.Add(Restrictions.Gt("MultaFgts", 0.0));

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
                criteria.CreateAlias("Loja", "Loja");

                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("AvisoPrevio"), "AvisoPrevio")
                    .Add(Projections.Sum("DecimoTerceiro"), "DecimoTerceiro")
                    .Add(Projections.Sum("MultaFgts"), "MultaFgts")
                    .Add(Projections.GroupProperty("Funcionario"), "Funcionario")
                    .Add(Projections.GroupProperty("Loja"), "Loja")
                    );

                //Quando há demissão os provisionamentos são zerados ao inserir um provisionamento com valores negativos para zerar os campos.
                //Neste caso para não listar os provisionamentos já resgatados, somente listo se o campo de soma de multa do fgts for maior que zero.
                //Esta soma nunca será zero a não ser que o provisionamento tenha sido resgatado.
                criteria.Add(Restrictions.Gt(Projections.Sum("MultaFgts"), 0.0));

                criteria.SetResultTransformer(Transformers.AliasToBean<Provisionamento>());

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "retorna provisionamentos totais de cada funcionario");
                throw new Exception($"Erro ao retornar provisionamentos totais de cada funcionario. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<double> GetTotalProvisionadoPorFuncionario(Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Deletado", false));
                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("AvisoPrevio"), "AvisoPrevio")
                    );

                return await criteria.UniqueResultAsync<double>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "retorna provisionamento total de um funcionario");
                throw new Exception($"Erro ao retornar provisionamento total de um funcionario. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<int> GetNumeroProvisionadoPorFuncionario(Funcionario funcionario)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Funcionario", funcionario));
                criteria.Add(Restrictions.Eq("Deletado", false));
                criteria.Add(Restrictions.Gt("AvisoPrevio", 0.0));
                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Count(Projections.Id())));

                return await criteria.UniqueResultAsync<int>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "retorna provisionamento total de um funcionario");
                throw new Exception($"Erro ao retornar provisionamento total de um funcionario. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
