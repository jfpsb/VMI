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
            var sessionRemota = SessionProvider.GetRemoteSession();

            var lojaDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("loja");
            var fornecedorDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("fornecedor");
            var marcaDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("marca");
            var tipoGradeDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("tipograde");
            var tipoContagemDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("tipocontagem");
            var contagemDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("contagem");
            var funcionarioDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("funcionario");
            var folhaPagamentoDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("folhapagamento");
            var operadoraCartaoDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("operadoracartao");
            var adiantamentoDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("adiantamento");
            var recebimentoCartaoDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("recebimentocartao");
            var produtoDocs = await CouchDbClient.Instancia.ListarDocumentosNaoSincronizados("produto");

            ProcessaDocsLocalComRemoto(lojaDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(fornecedorDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(marcaDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(tipoGradeDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(tipoContagemDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(contagemDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(funcionarioDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(folhaPagamentoDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(operadoraCartaoDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(adiantamentoDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(recebimentoCartaoDocs, sessionLocal, sessionRemota);
            ProcessaDocsLocalComRemoto(produtoDocs, sessionLocal, sessionRemota);

            SessionProvider.FechaSession(sessionRemota);
        }

        private async static void ProcessaDocsLocalComRemoto(CouchDbLogFindResult docs, ISession sessionLocal, ISession sessionRemota)
        {
            foreach (var doc in docs.Docs)
            {
                if (!doc.Replicado)
                {
                    switch (doc.Operacao)
                    {
                        case "insert":
                            try
                            {
                                var insert = await sessionLocal.GetAsync<Model.Produto>(doc.Id);
                                await sessionLocal.EvictAsync(insert);
                                await sessionRemota.SaveAsync(insert);
                                await sessionRemota.FlushAsync();
                                doc.Replicado = true;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"ERRO INSERT SYNC: {ex.Message}\n{ex.StackTrace}");
                                return;
                            }
                            break;
                        case "update":
                            try
                            {
                                var update = await sessionLocal.GetAsync<Model.Produto>(doc.Id);
                                await sessionLocal.EvictAsync(update);
                                await sessionRemota.UpdateAsync(update);
                                await sessionRemota.FlushAsync();
                                doc.Replicado = true;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"ERRO UPDATE SYNC: {ex.Message}\n{ex.StackTrace}");
                                return;
                            }
                            break;
                        case "delete":
                            try
                            {
                                var delete = await sessionLocal.GetAsync<Model.Produto>(doc.Id);
                                await sessionLocal.EvictAsync(delete);
                                await sessionRemota.DeleteAsync(delete);
                                await sessionRemota.FlushAsync();
                                doc.Replicado = true;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"ERRO DELETE SYNC: {ex.Message}\n{ex.StackTrace}");
                                return;
                            }
                            break;
                    }
                }

                if (doc.Replicado)
                {
                    //Salva em log que log foi replicado para banco de dados remoto
                    var replicadoResponse = await CouchDbClient.Instancia.UpdateDocument(doc);

                    if (replicadoResponse.Ok)
                    {
                        doc.Sincronizado = true;
                        doc.UltimaAlteracao = DateTime.Now;
                        var sincronizadoResponse = await CouchDbClient.Instancia.UpdateDocument(doc);

                        if (sincronizadoResponse.Ok)
                        {
                            //TODO: enviar logs para servidor
                        }
                    }
                }
            }
        }
    }
}
