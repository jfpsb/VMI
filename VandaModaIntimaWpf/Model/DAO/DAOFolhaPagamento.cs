using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFolhaPagamento : DAO<FolhaPagamento>
    {
        public DAOFolhaPagamento(ISession session) : base(session)
        {
        }

        public async Task<FolhaPagamento> ListarPorMesAnoFuncionario(Funcionario funcionario, int mes, int ano)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Funcionario", funcionario))
                    .Add(Restrictions.Eq("Mes", mes))
                    .Add(Restrictions.Eq("Ano", ano));

                return (FolhaPagamento)await criteria.UniqueResultAsync();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de folhas por funcionario");
                throw new Exception($"Erro ao listar folhas de pagamento. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task FecharFolhaDePagamento(FolhaPagamento folhaPagamento)
        {
            using (var tx = session.BeginTransaction())
            {
                try
                {

                    await session.SaveOrUpdateAsync(folhaPagamento);

                    if (folhaPagamento.Parcelas.Count > 0)
                    {
                        foreach (var parcela in folhaPagamento.Parcelas)
                            await session.UpdateAsync(parcela);
                    }

                    if (folhaPagamento.Bonus.Count > 0)
                    {
                        foreach (var bonus in folhaPagamento.Bonus)
                            await session.SaveOrUpdateAsync(bonus);
                    }

                    await tx.CommitAsync();

                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Log.EscreveLogBanco(ex, "fechar folha de pagamento");
                    throw new Exception($"Erro ao fechar folha de pagamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public async Task<bool> FecharFolhasDePagamento(IList<FolhaPagamento> folhas, IList<Parcela> parcelas, IList<Bonus> bonus)
        {
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    foreach (var folha in folhas)
                        await session.SaveOrUpdateAsync(folha);

                    if (parcelas.Count > 0)
                    {
                        foreach (var parcela in parcelas)
                            await session.UpdateAsync(parcela);
                    }

                    if (bonus.Count > 0)
                    {
                        foreach (var b in bonus)
                            await session.SaveOrUpdateAsync(b);
                    }

                    await tx.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Log.EscreveLogBanco(ex, "fechar folhas de pagamento");
                    throw new Exception($"Erro ao fechar folhas de pagamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
    }
}
