using NHibernate;
using NHibernate.Event;
using System;
using System.Threading;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class AdiantamentoPostEventListener : IPostUpdateEventListener
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
            var model = entity as AModel;

            if (model is Adiantamento)
            {
                var ad = model as Adiantamento;

                if (!model.Deletado)
                    return;

                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var parcs in ad.Parcelas)
                            {
                                parcs.Deletado = true;
                                await statelessSession.UpdateAsync(parcs);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando parcelas como deletada");
                            throw new Exception($"Erro ao deletar parcelas de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
        }
    }
}
