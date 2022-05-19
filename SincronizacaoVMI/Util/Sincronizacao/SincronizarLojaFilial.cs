using NHibernate;
using NHibernate.Criterion;
using SincronizacaoVMI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SincronizacaoVMI.Util
{
    public class SincronizarLojaFilial : ASincronizar<Loja>
    {
        public SincronizarLojaFilial(ISession local, ISession remoto, bool isChaveAutoIncremento = true) : base(local, remoto, isChaveAutoIncremento)
        {
        }

        public async override Task Sincronizar()
        {
            IList<Loja> insertsRemotoParaLocal = new List<Loja>();
            IList<Loja> updatesRemotoParaLocal = new List<Loja>();
            IList<Loja> insertsLocalParaRemoto = new List<Loja>();
            IList<Loja> updatesLocalParaRemoto = new List<Loja>();

            Console.WriteLine($"Iniciando sincronização de Lojas Filiais");

            var inicioSync = DateTime.Now;
            var lastSync = await GetLastSyncTime("lojafilial");

            if (lastSync == null)
            {
                lastSync = new LastSync
                {
                    Tabela = "lojafilial",
                    LastSyncTime = DateTime.MinValue
                };
            }

            //Lista entidades em banco remoto para insert em local
            try
            {
                var criteria = _remoto.CreateCriteria<Loja>();
                criteria.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                criteria.Add(Restrictions.IsNotNull("Matriz"));
                var lista = await criteria.ListAsync<Loja>();

                if (lista.Count > 0)
                {
                    double i = 0.0;
                    foreach (Loja e in lista)
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
                        Console.WriteLine($"Inserindo Loja Filial de banco remoto para local -> Copiando dados -> Progresso: {Math.Round(i++ / lista.Count * 100, 2)}%");
                    }

                    Console.WriteLine($"Inserindo Loja Filial de banco remoto para local -> Copiando dados -> Progresso: {Math.Round(i++ / lista.Count * 100, 2)}%");
                    await InsertRemotoParaLocal(insertsRemotoParaLocal);
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
                criteria.Add(Restrictions.IsNotNull("Matriz"));
                var lista = await criteria.ListAsync<Loja>();

                if (lista.Count > 0)
                {
                    double i = 0.0;
                    foreach (Loja e in lista)
                    {
                        if (e == null) continue;
                        Loja entLocal = await ListarPorUuidLocal("Loja", e.Uuid) as Loja;
                        if (entLocal == null) continue;
                        entLocal.Copiar(e);

                        Loja matrizLocal = await ListarPorUuidLocal("Loja", e.Matriz.Uuid) as Loja;
                        entLocal.Matriz = matrizLocal;

                        updatesRemotoParaLocal.Add(entLocal);
                        Console.WriteLine($"Atualizando Loja Filial de banco remoto para local -> Copiando dados -> Progresso: {Math.Round(i++ / lista.Count * 100, 2)}%");
                    }
                    Console.WriteLine($"Atualizando Loja Filial de banco remoto para local -> Copiando dados -> Progresso: {Math.Round(i++ / lista.Count * 100, 2)}%");
                    await UpdateRemotoParaLocal(updatesRemotoParaLocal);

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
                criteria.Add(Restrictions.IsNotNull("Matriz"));
                var lista = await criteria.ListAsync<Loja>();

                if (lista.Count > 0)
                {
                    double i = 0.0;
                    foreach (Loja e in lista)
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
                        Console.WriteLine($"Inserindo Loja Filial de banco local para remoto -> Copiando dados -> Progresso: {Math.Round(i++ / lista.Count * 100, 2)}%");
                    }

                    Console.WriteLine($"Inserindo Loja Filial de banco local para remoto -> Copiando dados -> Progresso: {Math.Round(i++ / lista.Count * 100, 2)}%");
                    await InsertLocalParaRemoto(insertsLocalParaRemoto);
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
                criteria.Add(Restrictions.IsNotNull("Matriz"));
                var lista = await criteria.ListAsync<Loja>();

                if (lista.Count > 0)
                {
                    double i = 0.0;
                    foreach (Loja e in lista)
                    {
                        if (e == null) continue;
                        Loja entRemoto = await ListarPorUuidRemoto("Loja", e.Uuid) as Loja;
                        if (entRemoto == null) continue;
                        entRemoto.Copiar(e);

                        Loja matrizRemoto = await ListarPorUuidRemoto("Loja", e.Matriz.Uuid) as Loja;
                        entRemoto.Matriz = matrizRemoto;

                        updatesLocalParaRemoto.Add(entRemoto);
                        Console.WriteLine($"Atualizando Loja Filial de banco local para remoto -> Copiando dados -> Progresso: {Math.Round(i++ / lista.Count * 100, 2)}%");
                    }

                    Console.WriteLine($"Atualizando Loja Filial de banco local para remoto -> Copiando dados -> Progresso: {Math.Round(i++ / lista.Count * 100, 2)}%");
                    await UpdateLocalParaRemoto(updatesLocalParaRemoto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista Loja em banco local para update em remoto");
                throw;
            }

            await SaveLastSyncTime(lastSync, inicioSync);
        }
    }
}
