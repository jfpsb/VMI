using NHibernate;
using NHibernate.Event;
using System;
using System.Threading;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class EntradaDeMercadoriaPostEventListener : IPostUpdateEventListener
    {
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            PostUpdate(@event);
        }

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            PostUpdate(@event);
            return Task.FromResult(false);
        }

        private async void PostUpdate(PostUpdateEvent @event)
        {
            var entity = @event.Entity;

            if (entity is EntradaDeMercadoria)
            {
                var model = entity as EntradaDeMercadoria;
                if (!model.Deletado)
                    return;
                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var em in model.Entradas)
                            {
                                em.Deletado = true;
                                await statelessSession.UpdateAsync(em);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
        }
    }
}
