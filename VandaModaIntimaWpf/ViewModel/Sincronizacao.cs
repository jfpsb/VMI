using Newtonsoft.Json;
using NHibernate;
using System;
using VandaModaIntimaWpf.BancoDeDados;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.BancoDeDados.CouchDb;

namespace VandaModaIntimaWpf.ViewModel
{
    public static class Sincronizacao
    {
        public async static void SincronizaLocalComRemoto(ISession sessionLocal)
        {
            var docs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("produto");
            var sessionRemota = SessionProvider.GetRemoteSession();

            foreach (var doc in docs.Docs)
            {
                bool synced = false;

                switch (doc.Operacao)
                {
                    case "insert":
                        var insert = await sessionLocal.GetAsync<Model.Produto>(doc.Id);
                        await sessionLocal.EvictAsync(insert);
                        await sessionRemota.ReplicateAsync(insert, ReplicationMode.Overwrite);
                        await sessionRemota.FlushAsync();
                        synced = true;
                        break;
                    case "update":
                        var update = await sessionLocal.GetAsync<Model.Produto>(doc.Id);
                        await sessionLocal.EvictAsync(update);
                        await sessionRemota.ReplicateAsync(update, ReplicationMode.Overwrite);
                        await sessionRemota.FlushAsync();
                        synced = true;
                        break;
                    case "delete":
                        var delete = await sessionLocal.GetAsync<Model.Produto>(doc.Id);
                        await sessionLocal.EvictAsync(delete);
                        await sessionRemota.DeleteAsync(delete);
                        await sessionRemota.FlushAsync();
                        synced = true;
                        break;
                }

                if (synced)
                {
                    doc.Sincronizado = true;
                    doc.UltimaAlteracao = DateTime.Now;
                    await CouchDbClient.Instancia.UpdateDocument(doc);
                }
            }

            SessionProvider.FechaSession(sessionLocal);
            SessionProvider.FechaSession(sessionRemota);
        }
    }
}
