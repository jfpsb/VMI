using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;
using SincronizacaoVMI.Banco;
using SincronizacaoVMI.Model;
using SincronizacaoVMI.Util;
using SincronizacaoVMI.Util.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SincronizacaoServico.Util
{
    /// <summary>
    /// Usado para sincronizar entidades sem chaves estrangeiras.
    /// </summary>
    public class SincronizarGenerico<E> : ASincronizar<E> where E : AModel, IModel, new()
    {
        public SincronizarGenerico(ISession local, ISession remoto, bool isChaveAutoIncremento = true) : base(local, remoto, isChaveAutoIncremento)
        {
        }

        public async override Task Sincronizar()
        {
            IList<E> insertsRemotoParaLocal = new List<E>();
            IList<E> updatesRemotoParaLocal = new List<E>();
            IList<E> insertsLocalParaRemoto = new List<E>();
            IList<E> updatesLocalParaRemoto = new List<E>();

            Console.WriteLine($"Iniciando sincronização de {typeof(E).Name}");

            var inicioSync = DateTime.Now;
            var lastSync = await GetLastSyncTime(typeof(E).Name.ToLower());

            if (lastSync == null)
            {
                lastSync = new LastSync
                {
                    Tabela = typeof(E).Name.ToLower(),
                    LastSyncTime = DateTime.MinValue
                };
            }

            //Lista entidades em banco remoto para insert em local
            try
            {
                var criteria = _remoto.CreateCriteria<E>();
                criteria.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                var lista = await criteria.ListAsync<E>();

                if (lista.Count > 0)
                {
                    var persister = SessionProviderBackup.BackupSessionFactory.GetClassMetadata(typeof(E));

                    foreach (E e in lista)
                    {
                        if (e == null) continue;
                        //Entidade com mesmo UUID no banco local
                        var ent = await ListarPorUuidLocal(typeof(E).Name, e.Uuid);
                        if (ent != null) continue;
                        E eASalvar = new();
                        eASalvar.Copiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.Where(w => persister.GetPropertyType(w).Equals(typeof(ManyToOneType)));
                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                var manyToOneLocal = await ListarPorUuidLocal(property, manyToOneValue.Uuid);

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                persister.SetPropertyValue(eASalvar, property, manyToOneLocal);
                            }
                        }

                        insertsRemotoParaLocal.Add(eASalvar);
                    }

                    await InsertRemotoParaLocal(insertsRemotoParaLocal);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista {typeof(E).Name.ToLower()} em banco remoto para insert em local");
                throw;
            }

            //Lista entidades em banco remoto para update em local
            try
            {
                var criteria = _remoto.CreateCriteria<E>();
                criteria.Add(Restrictions.Ge("ModificadoEm", lastSync.LastSyncTime));
                var lista = await criteria.ListAsync<E>();

                if (lista.Count > 0)
                {
                    var persister = SessionProviderBackup.BackupSessionFactory.GetClassMetadata(typeof(E));

                    foreach (E e in lista)
                    {
                        if (e == null) continue;
                        E entLocal = await ListarPorUuidLocal(typeof(E).Name, e.Uuid) as E;
                        if (entLocal == null) continue;
                        entLocal.Copiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.Where(w => persister.GetPropertyType(w).Equals(typeof(ManyToOneType)));
                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                var manyToOneLocal = await ListarPorUuidLocal(property, manyToOneValue.Uuid);

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                persister.SetPropertyValue(entLocal, property, manyToOneLocal);
                            }
                        }

                        updatesRemotoParaLocal.Add(entLocal);
                    }


                    await UpdateRemotoParaLocal(updatesRemotoParaLocal);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista {typeof(E).Name.ToLower()} em banco remoto para update em local");
                throw;
            }

            //Lista entidades em banco local para insert em remoto
            try
            {
                var criteria = _local.CreateCriteria<E>();
                criteria.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                var lista = await criteria.ListAsync<E>();

                if (lista.Count > 0)
                {
                    var persister = SessionProvider.SessionFactory.GetClassMetadata(typeof(E));
                    foreach (E e in lista)
                    {
                        if (e == null) continue;
                        //Entidade com mesmo UUID no banco remoto
                        var ent = await ListarPorUuidRemoto(typeof(E).Name, e.Uuid);
                        if (ent != null) continue;
                        E eASalvar = new();
                        eASalvar.Copiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.Where(w => persister.GetPropertyType(w).Equals(typeof(ManyToOneType)));
                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                var manyToOneLocal = await ListarPorUuidLocal(property, manyToOneValue.Uuid);

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                persister.SetPropertyValue(eASalvar, property, manyToOneLocal);
                            }
                        }

                        insertsLocalParaRemoto.Add(eASalvar);
                    }

                    await InsertLocalParaRemoto(insertsLocalParaRemoto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista {typeof(E).Name.ToLower()} em banco local para insert em remoto");
                throw;
            }

            //Lista entidades em banco local para update em remoto
            try
            {
                var criteria = _local.CreateCriteria<E>();
                criteria.Add(Restrictions.Ge("ModificadoEm", lastSync.LastSyncTime));
                var lista = await criteria.ListAsync<E>();

                if (lista.Count > 0)
                {
                    var persister = SessionProvider.SessionFactory.GetClassMetadata(typeof(E));
                    foreach (E e in lista)
                    {
                        if (e == null) continue;
                        E entRemoto = await ListarPorUuidRemoto(typeof(E).Name, e.Uuid) as E;
                        if (entRemoto == null) continue;
                        entRemoto.Copiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.Where(w => persister.GetPropertyType(w).Equals(typeof(ManyToOneType)));
                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                var manyToOneLocal = await ListarPorUuidLocal(property, manyToOneValue.Uuid);

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                persister.SetPropertyValue(entRemoto, property, manyToOneLocal);
                            }
                        }

                        updatesLocalParaRemoto.Add(entRemoto);
                    }

                    await UpdateLocalParaRemoto(updatesLocalParaRemoto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista {typeof(E).Name.ToLower()} em banco local para update em remoto");
                throw;
            }

            await SaveLastSyncTime(lastSync, inicioSync);
        }
    }
}
