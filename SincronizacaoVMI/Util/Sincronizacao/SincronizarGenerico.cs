using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;
using SincronizacaoVMI.BancoDeDados;
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
    public class SincronizarGenerico<E> : ASincronizar<E> where E : AModel, new()
    {
        public SincronizarGenerico(ISession local, ISession remoto, bool isChaveAutoIncremento = true) : base(local, remoto, isChaveAutoIncremento)
        {
        }

        public async override Task Sincronizar()
        {
            var inicioSync = DateTime.Now;

            IList<E> insertsRemotoParaLocal = new List<E>();
            IList<E> updatesRemotoParaLocal = new List<E>();
            IList<E> insertsLocalParaRemoto = new List<E>();
            IList<E> updatesLocalParaRemoto = new List<E>();

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
            //Lista entidades em banco remoto para update em local
            try
            {
                var criteriaInserts = _remoto.CreateCriteria<E>();
                criteriaInserts.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                var futureInserts = criteriaInserts.Future<E>();

                var criteriaUpdates = _remoto.CreateCriteria<E>();
                criteriaUpdates.Add(Restrictions.Ge("ModificadoEm", lastSync.LastSyncTime));
                var futureUpdates = criteriaUpdates.Future<E>();

                var persister = SessionProviderBackup.BackupSessionFactory.GetClassMetadata(typeof(E));

                Dictionary<string, object> futureInsertsDic = new Dictionary<string, object>();
                Dictionary<string, object> futureUpdatesDic = new Dictionary<string, object>();

                if (futureInserts.Any() && persister.PropertyTypes != null)
                {
                    foreach (E e in futureInserts.GetEnumerable())
                    {
                        if (e == null) continue;
                        var criteria = _local.CreateCriteria(typeof(E).Name);
                        criteria.Add(Restrictions.Eq("Uuid", e.Uuid));

                        var tipo = Type.GetType($"SincronizacaoVMI.Model.{typeof(E).Name}, SincronizacaoVMI", true);
                        var metodo = criteria.GetType().GetMethod("FutureValue");
                        var metodoGenerico = metodo.MakeGenericMethod(tipo);

                        futureInsertsDic.Add(e.Uuid.ToString(), metodoGenerico.Invoke(criteria, null));
                    }
                }

                if (futureInserts.Any() && persister.PropertyTypes != null)
                {
                    foreach (E e in futureInserts.GetEnumerable())
                    {
                        if (e == null) continue;

                        var propInsertValue = futureInsertsDic[e.Uuid.ToString()].GetType().GetProperty("Value");
                        var ent = propInsertValue.GetValue(futureInsertsDic[e.Uuid.ToString()]);

                        if (ent != null) continue;
                        E eASalvar = new();
                        eASalvar.Copiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.GetManyToOnePropertyNames(persister);

                            Dictionary<string, object> futureDictionary = new Dictionary<string, object>();

                            foreach (var property in manyToOneProperties)
                            {
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                if (manyToOneValue == null) continue;
                                if (futureDictionary.ContainsKey(manyToOneValue.Uuid.ToString())) continue;

                                ICriteria criteria = _local.CreateCriteria(persister.GetPropertyTypeSimpleName(property));
                                criteria.Add(Restrictions.Eq("Uuid", manyToOneValue.Uuid));

                                var tipo = Type.GetType($"SincronizacaoVMI.Model.{persister.GetPropertyTypeSimpleName(property)}, SincronizacaoVMI", true);
                                var metodo = criteria.GetType().GetMethod("FutureValue");
                                var metodoGenerico = metodo.MakeGenericMethod(tipo);

                                futureDictionary.Add(manyToOneValue.Uuid.ToString(), metodoGenerico.Invoke(criteria, null));
                            }

                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropertyIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;

                                object manyToOneLocal = null;
                                if (manyToOneValue != null)
                                {
                                    var propValue = futureDictionary[manyToOneValue.Uuid.ToString()].GetType().GetProperty("Value");
                                    manyToOneLocal = propValue.GetValue(futureDictionary[manyToOneValue.Uuid.ToString()]);
                                }

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                eASalvar.GetType().GetProperty(property).SetValue(eASalvar, manyToOneLocal);
                            }
                        }
                        insertsRemotoParaLocal.Add(eASalvar);
                    }
                }

                if (futureUpdates.Any() && persister.PropertyTypes != null)
                {
                    foreach (E e in futureUpdates.GetEnumerable())
                    {
                        if (e == null) continue;
                        var criteria = _local.CreateCriteria(typeof(E).Name);
                        criteria.Add(Restrictions.Eq("Uuid", e.Uuid));
                        var tipo = Type.GetType($"SincronizacaoVMI.Model.{typeof(E).Name}, SincronizacaoVMI", true);
                        var metodo = criteria.GetType().GetMethod("FutureValue");
                        var metodoGenerico = metodo.MakeGenericMethod(tipo);

                        futureUpdatesDic.Add(e.Uuid.ToString(), metodoGenerico.Invoke(criteria, null));
                    }
                }

                if (futureUpdates.Any() && persister.PropertyTypes != null)
                {
                    foreach (E e in futureUpdates.GetEnumerable())
                    {
                        if (e == null) continue;
                        var propUpdateValue = futureUpdatesDic[e.Uuid.ToString()].GetType().GetProperty("Value");
                        var entLocal = propUpdateValue.GetValue(futureUpdatesDic[e.Uuid.ToString()]) as E;
                        if (entLocal == null) continue;
                        entLocal.Copiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.GetManyToOnePropertyNames(persister);

                            Dictionary<string, object> updateDic = new Dictionary<string, object>();

                            foreach (var property in manyToOneProperties)
                            {
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                if (manyToOneValue == null) continue;
                                if (updateDic.ContainsKey(manyToOneValue.Uuid.ToString())) continue;

                                ICriteria criteria = _local.CreateCriteria(persister.GetPropertyTypeSimpleName(property));
                                criteria.Add(Restrictions.Eq("Uuid", manyToOneValue.Uuid));

                                var tipo = Type.GetType($"SincronizacaoVMI.Model.{persister.GetPropertyTypeSimpleName(property)}, SincronizacaoVMI", true);
                                var metodo = criteria.GetType().GetMethod("FutureValue");
                                var metodoGenerico = metodo.MakeGenericMethod(tipo);

                                updateDic.Add(manyToOneValue.Uuid.ToString(), metodoGenerico.Invoke(criteria, null));
                            }

                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropertyIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;

                                object manyToOneLocal = null;
                                if (manyToOneValue != null)
                                {
                                    var propValue = updateDic[manyToOneValue.Uuid.ToString()].GetType().GetProperty("Value");
                                    manyToOneLocal = propValue.GetValue(updateDic[manyToOneValue.Uuid.ToString()]);
                                }

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                entLocal.GetType().GetProperty(property).SetValue(entLocal, manyToOneLocal);
                            }
                        }

                        updatesRemotoParaLocal.Add(entLocal);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista {typeof(E).Name.ToLower()} em banco remoto para insert/update em local");
                throw;
            }

            //Lista entidades em banco local para insert em remoto
            //Lista entidades em banco local para update em remoto
            try
            {
                var criteriaInserts = _local.CreateCriteria<E>();
                criteriaInserts.Add(Restrictions.Ge("CriadoEm", lastSync.LastSyncTime));
                var futureInserts = criteriaInserts.Future<E>();

                var criteriaUpdates = _local.CreateCriteria<E>();
                criteriaUpdates.Add(Restrictions.Ge("ModificadoEm", lastSync.LastSyncTime));
                var futureUpdates = criteriaUpdates.Future<E>();

                var persister = SessionProvider.SessionFactory.GetClassMetadata(typeof(E));

                Dictionary<string, object> futureInsertsDic = new Dictionary<string, object>();
                Dictionary<string, object> futureUpdatesDic = new Dictionary<string, object>();

                if (futureInserts.Any() && persister.PropertyTypes != null)
                {
                    foreach (E e in futureInserts.GetEnumerable())
                    {
                        if (e == null) continue;
                        var criteria = _remoto.CreateCriteria(typeof(E).Name);
                        criteria.Add(Restrictions.Eq("Uuid", e.Uuid));

                        var tipo = Type.GetType($"SincronizacaoVMI.Model.{typeof(E).Name}, SincronizacaoVMI", true);
                        var metodo = criteria.GetType().GetMethod("FutureValue");
                        var metodoGenerico = metodo.MakeGenericMethod(tipo);

                        futureInsertsDic.Add(e.Uuid.ToString(), metodoGenerico.Invoke(criteria, null));
                    }
                }

                if (futureInserts.Any() && persister.PropertyTypes != null)
                {
                    foreach (E e in futureInserts.GetEnumerable())
                    {
                        if (e == null) continue;

                        var propInsertValue = futureInsertsDic[e.Uuid.ToString()].GetType().GetProperty("Value");
                        var ent = propInsertValue.GetValue(futureInsertsDic[e.Uuid.ToString()]);

                        if (ent != null) continue;
                        E eASalvar = new();
                        eASalvar.Copiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.GetManyToOnePropertyNames(persister);

                            Dictionary<string, object> futuredic = new Dictionary<string, object>();

                            foreach (var property in manyToOneProperties)
                            {
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                if (manyToOneValue == null) continue;
                                if (futuredic.ContainsKey(manyToOneValue.Uuid.ToString())) continue;

                                ICriteria criteria = _remoto.CreateCriteria(persister.GetPropertyTypeSimpleName(property));
                                criteria.Add(Restrictions.Eq("Uuid", manyToOneValue.Uuid));

                                var tipo = Type.GetType($"SincronizacaoVMI.Model.{persister.GetPropertyTypeSimpleName(property)}, SincronizacaoVMI", true);
                                var metodo = criteria.GetType().GetMethod("FutureValue");
                                var metodoGenerico = metodo.MakeGenericMethod(tipo);

                                futuredic.Add(manyToOneValue.Uuid.ToString(), metodoGenerico.Invoke(criteria, null));
                            }

                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropertyIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;

                                object manyToOneLocal = null;
                                if (manyToOneValue != null)
                                {
                                    //manyToOneLocal = await ListarPorUuidRemoto(persister.GetPropertyTypeSimpleName(property), manyToOneValue.Uuid);
                                    var propValue = futuredic[manyToOneValue.Uuid.ToString()].GetType().GetProperty("Value");
                                    manyToOneLocal = propValue.GetValue(futuredic[manyToOneValue.Uuid.ToString()]);
                                }

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }
                                eASalvar.GetType().GetProperty(property).SetValue(eASalvar, manyToOneLocal);
                            }
                        }

                        insertsLocalParaRemoto.Add(eASalvar);
                    }
                }

                if (futureUpdates.Any() && persister.PropertyTypes != null)
                {
                    foreach (E e in futureUpdates.GetEnumerable())
                    {
                        if (e == null) continue;
                        var criteria = _remoto.CreateCriteria(typeof(E).Name);
                        criteria.Add(Restrictions.Eq("Uuid", e.Uuid));
                        var tipo = Type.GetType($"SincronizacaoVMI.Model.{typeof(E).Name}, SincronizacaoVMI", true);
                        var metodo = criteria.GetType().GetMethod("FutureValue");
                        var metodoGenerico = metodo.MakeGenericMethod(tipo);

                        futureUpdatesDic.Add(e.Uuid.ToString(), metodoGenerico.Invoke(criteria, null));
                    }
                }

                if (futureUpdates.Any() && persister.PropertyTypes != null)
                {
                    foreach (E e in futureUpdates.GetEnumerable())
                    {
                        if (e == null) continue;
                        //E entRemoto = await ListarPorUuidRemoto(typeof(E).Name, e.Uuid) as E;
                        var propUpdateValue = futureUpdatesDic[e.Uuid.ToString()].GetType().GetProperty("Value");
                        var entRemoto = propUpdateValue.GetValue(futureUpdatesDic[e.Uuid.ToString()]) as E;
                        if (entRemoto == null) continue;
                        entRemoto.Copiar(e);

                        if (persister.PropertyTypes.ContainsType(typeof(ManyToOneType)))
                        {
                            var manyToOneProperties = persister.PropertyNames.GetManyToOnePropertyNames(persister);

                            Dictionary<string, object> updateDic = new Dictionary<string, object>();

                            foreach (var property in manyToOneProperties)
                            {
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;
                                if (manyToOneValue == null) continue;
                                if (updateDic.ContainsKey(manyToOneValue.Uuid.ToString())) continue;

                                ICriteria criteria = _remoto.CreateCriteria(persister.GetPropertyTypeSimpleName(property));
                                criteria.Add(Restrictions.Eq("Uuid", manyToOneValue.Uuid));

                                var tipo = Type.GetType($"SincronizacaoVMI.Model.{persister.GetPropertyTypeSimpleName(property)}, SincronizacaoVMI", true);
                                var metodo = criteria.GetType().GetMethod("FutureValue");
                                var metodoGenerico = metodo.MakeGenericMethod(tipo);

                                updateDic.Add(manyToOneValue.Uuid.ToString(), metodoGenerico.Invoke(criteria, null));
                            }

                            foreach (var property in manyToOneProperties)
                            {
                                int propertyIndex = persister.PropertyNames.PropertyIndex(property);
                                bool isPropNullable = persister.PropertyNullability[propertyIndex];
                                var manyToOneValue = persister.GetPropertyValue(e, property) as AModel;

                                object manyToOneLocal = null;
                                if (manyToOneValue != null)
                                {
                                    //manyToOneLocal = await ListarPorUuidRemoto(persister.GetPropertyTypeSimpleName(property), manyToOneValue.Uuid);
                                    var propValue = updateDic[manyToOneValue.Uuid.ToString()].GetType().GetProperty("Value");
                                    //var t2 = updateDic[manyToOneValue.Uuid.ToString()];
                                    manyToOneLocal = propValue.GetValue(updateDic[manyToOneValue.Uuid.ToString()]);
                                }

                                if (isPropNullable == false && manyToOneLocal == null)
                                {
                                    throw new Exception($"{property} não pode ser nulo.");
                                }

                                entRemoto.GetType().GetProperty(property).SetValue(entRemoto, manyToOneLocal);
                            }
                        }

                        updatesLocalParaRemoto.Add(entRemoto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
                Log.EscreveLogSync(ex, $"Lista {typeof(E).Name.ToLower()} em banco local para insert/update em remoto");
                throw;
            }

            var res1 = await InsertRemotoParaLocal(insertsRemotoParaLocal);
            var res2 = await UpdateRemotoParaLocal(updatesRemotoParaLocal);
            var res3 = await InsertLocalParaRemoto(insertsLocalParaRemoto);
            var res4 = await UpdateLocalParaRemoto(updatesLocalParaRemoto);
            if (res1 || res2 || res3 || res4)
                await SaveLastSyncTime(lastSync, inicioSync);
        }
    }
}
