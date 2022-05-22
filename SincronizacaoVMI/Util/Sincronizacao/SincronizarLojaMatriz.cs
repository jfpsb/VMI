using NHibernate;
using NHibernate.Criterion;
using SincronizacaoVMI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SincronizacaoVMI.Util
{
    public class SincronizarLojaMatriz : ASincronizar<Loja>
    {
        public SincronizarLojaMatriz(ISession local, ISession remoto, bool isChaveAutoIncremento = true) : base(local, remoto, isChaveAutoIncremento)
        {
        }

        public async override Task Sincronizar()
        {
            IList<Loja> insertsRemotoParaLocal = new List<Loja>();
            IList<Loja> updatesRemotoParaLocal = new List<Loja>();
            IList<Loja> insertsLocalParaRemoto = new List<Loja>();
            IList<Loja> updatesLocalParaRemoto = new List<Loja>();

            var inicioSync = DateTime.Now;
            var lastSync = await GetLastSyncTime("lojamatriz");

            if (lastSync == null)
            {
                lastSync = new LastSync
                {
                    Tabela = "lojamatriz",
                    LastSyncTime = DateTime.MinValue
                };
            }

            //Lista entidades em banco remoto para insert em local
            //Lista entidades em banco remoto para update em local
            try
            {
                var criteriaInserts = _remoto.CreateCriteria<Loja>();
                criteriaInserts.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                criteriaInserts.Add(Restrictions.IsNull("Matriz"));
                var futureInserts = criteriaInserts.Future<Loja>();

                var criteriaUpdates = _remoto.CreateCriteria<Loja>();
                criteriaUpdates.Add(Restrictions.Ge("ModificadoEm", lastSync.LastSyncTime));
                criteriaUpdates.Add(Restrictions.IsNull("Matriz"));
                var futureUpdates = criteriaUpdates.Future<Loja>();

                if (futureInserts.Any())
                {
                    Console.WriteLine($"LojaMatriz - Encontrado(s) {futureInserts.GetEnumerable().Count()} itens para inserção remoto para local.");
                    foreach (Loja e in futureInserts.GetEnumerable())
                    {
                        if (e == null) continue;
                        //Entidade com mesmo UUID no banco local
                        var ent = await ListarPorUuidLocal("Loja", e.Uuid);
                        if (ent != null) continue;
                        Loja eASalvar = new Loja();
                        eASalvar.Copiar(e);

                        Loja matrizLocal = await ListarPorUuidLocal("Loja", e.Matriz.Uuid) as Loja;
                        eASalvar.Matriz = matrizLocal;

                        insertsRemotoParaLocal.Add(eASalvar);
                    }
                }

                if (futureUpdates.Any())
                {
                    Console.WriteLine($"LojaMatriz - Encontrado(s) {futureUpdates.GetEnumerable().Count()} itens para atualização remoto para local.");
                    foreach (Loja e in futureUpdates.GetEnumerable())
                    {
                        if (e == null) continue;
                        Loja entLocal = await ListarPorUuidLocal("Loja", e.Uuid) as Loja;
                        if (entLocal == null) continue;
                        entLocal.Copiar(e);

                        Loja matrizLocal = await ListarPorUuidLocal("Loja", e.Matriz.Uuid) as Loja;
                        entLocal.Matriz = matrizLocal;

                        updatesRemotoParaLocal.Add(entLocal);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista LojaMatriz em banco remoto para insert/update em local");
                throw;
            }

            //Lista entidades em banco local para insert em remoto
            //Lista entidades em banco local para update em remoto
            try
            {
                var criteriaInserts = _local.CreateCriteria<Loja>();
                criteriaInserts.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                criteriaInserts.Add(Restrictions.IsNull("Matriz"));
                var futureInserts = criteriaInserts.Future<Loja>();

                var criteriaUpdates = _local.CreateCriteria<Loja>();
                criteriaUpdates.Add(Restrictions.Ge("ModificadoEm", lastSync.LastSyncTime));
                criteriaUpdates.Add(Restrictions.IsNull("Matriz"));
                var futureUpdates = criteriaUpdates.Future<Loja>();

                if (futureInserts.Any())
                {
                    Console.WriteLine($"LojaMatriz - Encontrado(s) {futureInserts.GetEnumerable().Count()} itens para inserção local para remoto.");
                    foreach (Loja e in futureInserts.GetEnumerable())
                    {
                        if (e == null) continue;
                        //Entidade com mesmo UUID no banco remoto
                        var ent = await ListarPorUuidRemoto("Loja", e.Uuid);
                        if (ent != null) continue;
                        Loja eASalvar = new();
                        eASalvar.Copiar(e);

                        Loja matrizRemoto = await ListarPorUuidRemoto("Loja", e.Matriz.Uuid) as Loja;
                        eASalvar.Matriz = matrizRemoto;

                        insertsLocalParaRemoto.Add(eASalvar);
                    }
                }

                if (futureUpdates.Any())
                {
                    Console.WriteLine($"LojaMatriz - Encontrado(s) {futureUpdates.GetEnumerable().Count()} itens para atualização local para remoto.");
                    foreach (Loja e in futureUpdates.GetEnumerable())
                    {
                        if (e == null) continue;
                        Loja entRemoto = await ListarPorUuidRemoto("Loja", e.Uuid) as Loja;
                        if (entRemoto == null) continue;
                        entRemoto.Copiar(e);

                        Loja matrizRemoto = await ListarPorUuidRemoto("Loja", e.Matriz.Uuid) as Loja;
                        entRemoto.Matriz = matrizRemoto;

                        updatesLocalParaRemoto.Add(entRemoto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista LojaMatriz em banco local para insert/update em remoto");
                throw;
            }

            await InsertRemotoParaLocal(insertsRemotoParaLocal);
            await UpdateRemotoParaLocal(updatesRemotoParaLocal);
            await InsertLocalParaRemoto(insertsLocalParaRemoto);
            await UpdateLocalParaRemoto(updatesLocalParaRemoto);
            if (insertsRemotoParaLocal.Count > 0 || updatesRemotoParaLocal.Count > 0 || insertsLocalParaRemoto.Count > 0 || updatesLocalParaRemoto.Count > 0)
                await SaveLastSyncTime(lastSync, inicioSync);
        }
    }
}
