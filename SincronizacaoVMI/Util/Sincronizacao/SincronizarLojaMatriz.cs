using NHibernate;
using NHibernate.Criterion;
using SincronizacaoVMI.Model;
using System;
using System.Collections.Generic;
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
            try
            {
                var criteria = _remoto.CreateCriteria<Loja>();
                criteria.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                criteria.Add(Restrictions.IsNull("Matriz"));
                var lista = await criteria.ListAsync<Loja>();

                if (lista.Count > 0)
                {
                    Console.WriteLine($"LojaMatriz - Encontrado(s) {lista.Count} itens para inserção remoto para local.");
                    foreach (Loja e in lista)
                    {
                        if (e == null) continue;
                        //Entidade com mesmo UUID no banco local
                        var ent = await ListarPorUuidLocal("Loja", e.Uuid);
                        if (ent != null) continue;
                        Loja eASalvar = new Loja();
                        eASalvar.Copiar(e);
                        insertsRemotoParaLocal.Add(eASalvar);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista Loja em banco remoto para insert em local");
                throw;
            }

            //Lista entidades em banco remoto para update em local
            try
            {
                var criteria = _remoto.CreateCriteria<Loja>();
                criteria.Add(Restrictions.Ge("ModificadoEm", lastSync.LastSyncTime));
                criteria.Add(Restrictions.IsNull("Matriz"));
                var lista = await criteria.ListAsync<Loja>();

                if (lista.Count > 0)
                {
                    Console.WriteLine($"LojaFilial - Encontrado(s) {lista.Count} itens para atualização remoto para local.");
                    foreach (Loja e in lista)
                    {
                        if (e == null) continue;
                        Loja entLocal = await ListarPorUuidLocal("Loja", e.Uuid) as Loja;
                        if (entLocal == null) continue;
                        entLocal.Copiar(e);
                        updatesRemotoParaLocal.Add(entLocal);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista Loja em banco remoto para update em local");
                throw;
            }

            //Lista entidades em banco local para insert em remoto
            try
            {
                var criteria = _local.CreateCriteria<Loja>();
                criteria.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                criteria.Add(Restrictions.IsNull("Matriz"));
                var lista = await criteria.ListAsync<Loja>();

                if (lista.Count > 0)
                {
                    Console.WriteLine($"LojaFilial - Encontrado(s) {lista.Count} itens para inserção local para remoto.");
                    foreach (Loja e in lista)
                    {
                        if (e == null) continue;
                        //Entidade com mesmo UUID no banco remoto
                        var ent = await ListarPorUuidRemoto("Loja", e.Uuid);
                        if (ent != null) continue;
                        Loja eASalvar = new();
                        eASalvar.Copiar(e);
                        insertsLocalParaRemoto.Add(eASalvar);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista Loja em banco local para insert em remoto");
                throw;
            }

            //Lista entidades em banco local para update em remoto
            try
            {
                var criteria = _local.CreateCriteria<Loja>();
                criteria.Add(Restrictions.Ge("ModificadoEm", lastSync.LastSyncTime));
                criteria.Add(Restrictions.IsNull("Matriz"));
                var lista = await criteria.ListAsync<Loja>();

                if (lista.Count > 0)
                {
                    Console.WriteLine($"LojaFilial - Encontrado(s) {lista.Count} itens para atualização local para remoto.");
                    foreach (Loja e in lista)
                    {
                        if (e == null) continue;
                        Loja entRemoto = await ListarPorUuidRemoto("Loja", e.Uuid) as Loja;
                        if (entRemoto == null) continue;
                        entRemoto.Copiar(e);
                        updatesLocalParaRemoto.Add(entRemoto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista Loja em banco local para update em remoto");
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
