using NHibernate;
using System;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOAdiantamento : DAO
    {
        public DAOAdiantamento(ISession session) : base(session)
        {
        }

        public override async Task<object> Inserir(object objeto)
        {
            Adiantamento adiantamento = objeto as Adiantamento;

            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (Parcela p in adiantamento.Parcelas)
                    {
                        await session.SaveAsync(p.FolhaPagamento);
                    }

                    var result = await session.SaveAsync(objeto);
                    await transacao.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO INSERIR >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR >>> " + ex.InnerException.Message);
                }

                return null;
            }
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.LoadAsync<Adiantamento>(id);
        }
    }
}
