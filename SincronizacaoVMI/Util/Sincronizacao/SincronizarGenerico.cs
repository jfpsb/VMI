using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;
using SincronizacaoVMI.Banco;
using SincronizacaoVMI.Model;
using SincronizacaoVMI.Util.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SincronizacaoVMI.Util
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
                var persister = SessionProviderBackup.BackupSessionFactory.GetClassMetadata(typeof(E));

                if (lista.Count > 0 && persister.PropertyTypes != null)
                {
                    foreach (E e in lista)
                    {
                        if (e == null) continue;
                        //Entidade com mesmo UUID no banco local
                        var ent = await ListarPorUuidLocal(typeof(E).Name, e.Uuid);
                        if (ent != null) continue;
                        E eASalvar = new();
                        eASalvar.NovoCopiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.Where(w => persister.GetPropertyType(w).Equals(typeof(ManyToOneType)));
                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropertyIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                var manyToOneLocal = await ListarPorUuidLocal(property, manyToOneValue.Uuid);

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                eASalvar.GetType().GetProperty(property).SetValue(eASalvar, manyToOneLocal);
                                //persister.SetPropertyValue(eASalvar, property, manyToOneLocal);
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
                var persister = SessionProviderBackup.BackupSessionFactory.GetClassMetadata(typeof(E));

                if (lista.Count > 0 && persister.PropertyTypes != null)
                {
                    foreach (E e in lista)
                    {
                        if (e == null) continue;
                        E entLocal = await ListarPorUuidLocal(typeof(E).Name, e.Uuid) as E;
                        if (entLocal == null) continue;
                        entLocal.NovoCopiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.Where(w => persister.GetPropertyType(w).Equals(typeof(ManyToOneType)));
                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropertyIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                var manyToOneLocal = await ListarPorUuidLocal(property, manyToOneValue.Uuid);

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                entLocal.GetType().GetProperty(property).SetValue(entLocal, manyToOneLocal);
                                //persister.SetPropertyValue(entLocal, property, manyToOneLocal);
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
                var persister = SessionProvider.SessionFactory.GetClassMetadata(typeof(E));

                if (lista.Count > 0 && persister.PropertyTypes != null)
                {
                    foreach (E e in lista)
                    {
                        if (e == null) continue;
                        //Entidade com mesmo UUID no banco remoto
                        var ent = await ListarPorUuidRemoto(typeof(E).Name, e.Uuid);
                        if (ent != null) continue;
                        E eASalvar = new();
                        eASalvar.NovoCopiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.GetManyToOnePropertyNames(persister);
                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropertyIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                if (manyToOneValue == null) continue;
                                var manyToOneLocal = await ListarPorUuidRemoto(persister.GetPropertyTypeSimpleName(property), manyToOneValue.Uuid);
                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }
                                eASalvar.GetType().GetProperty(property).SetValue(eASalvar, manyToOneLocal);
                                //persister.SetPropertyValue(eASalvar, property, manyToOneLocal);
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
                var persister = SessionProvider.SessionFactory.GetClassMetadata(typeof(E));

                if (lista.Count > 0 && persister.PropertyTypes != null)
                {
                    foreach (E e in lista)
                    {
                        if (e == null) continue;
                        E entRemoto = await ListarPorUuidRemoto(typeof(E).Name, e.Uuid) as E;
                        if (entRemoto == null) continue;
                        entRemoto.NovoCopiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.Where(w => persister.GetPropertyType(w).Equals(typeof(ManyToOneType)));
                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropertyIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                var manyToOneLocal = await ListarPorUuidRemoto(property, manyToOneValue.Uuid);

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                entRemoto.GetType().GetProperty(property).SetValue(entRemoto, manyToOneLocal);
                                //persister.SetPropertyValue(entRemoto, property, manyToOneLocal);
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
