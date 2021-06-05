using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFolhaPagamento : DAO<FolhaPagamento>
    {
        public DAOFolhaPagamento(ISession session) : base(session)
        {
        }

        public async Task<FolhaPagamento> ListarPorMesAnoFuncionario(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("Funcionario", funcionario))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Ano", ano));

            return (FolhaPagamento)await criteria.UniqueResultAsync();
        }

        public async Task<bool> FecharFolhaDePagamento(FolhaPagamento folhaPagamento, IList<Parcela> parcelas)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.SaveOrUpdateAsync(folhaPagamento);

                    if (parcelas.Count > 0)
                    {
                        foreach (var parcela in parcelas)
                            await session.UpdateAsync(parcela);
                    }

                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO INSERIR >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
        public async Task<bool> FecharFolhasDePagamento(IList<FolhaPagamento> folhas, IList<Parcela> parcelas)
        {
            using (var transacao = session.BeginTransaction())
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

                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO INSERIR >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
    }
}
