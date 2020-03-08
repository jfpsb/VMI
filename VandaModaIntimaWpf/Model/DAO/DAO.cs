using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util.Sincronizacao;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.Model.DAO
{
    public abstract class DAO
    {
        protected ISession session;
        public DAO(ISession session)
        {
            this.session = session;
        }

        public virtual async Task<bool> Inserir(object objeto, bool writeToJson = true, bool sendToServer = true)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.SaveAsync(objeto);

                    if (writeToJson)
                    {
                        var databaseLogFile = WriteDatabaseLogFile("INSERT", objeto);

                        if (sendToServer)
                            SendDatabaseLogFileToServer(databaseLogFile, objeto.GetType());
                    }

                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO INSERIR >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
        public virtual async Task<bool> Inserir<E>(IList<E> objetos, bool writeToJson = true, bool sendToServer = true) where E : class, IModel
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        await session.SaveOrUpdateAsync(e);
                    }

                    if (writeToJson)
                    {
                        foreach (E e in objetos)
                        {
                            DatabaseLogFile<E> databaseLogFile = SincronizacaoViewModel.WriteDatabaseLogFile<E>("UPDATE", e);

                            if (sendToServer)
                                SincronizacaoViewModel.SendDatabaseLogFileToServer(databaseLogFile);
                        }
                    }

                    await transacao.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO INSERIR OU ATUALIZAR LISTA >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR OU ATUALIZAR LISTA >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
        public virtual async Task<bool> InserirOuAtualizar(object objeto, bool writeToJson = true, bool sendToServer = true)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    var o = await session.MergeAsync(objeto);
                    await session.SaveOrUpdateAsync(o);

                    if (writeToJson)
                    {
                        var databaseLogFile = WriteDatabaseLogFile("UPDATE", objeto);

                        if (sendToServer)
                            SendDatabaseLogFileToServer(databaseLogFile, objeto.GetType());
                    }

                    await transacao.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO INSERIR >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
        public virtual async Task<bool> InserirOuAtualizar<E>(IList<E> objetos, bool writeToJson = true, bool sendToServer = true) where E : class, IModel
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        await session.SaveOrUpdateAsync(e);
                    }

                    if (writeToJson)
                    {
                        foreach (E e in objetos)
                        {
                            DatabaseLogFile<E> databaseLogFile = SincronizacaoViewModel.WriteDatabaseLogFile("UPDATE", e);

                            if (sendToServer)
                                SincronizacaoViewModel.SendDatabaseLogFileToServer(databaseLogFile);
                        }
                    }

                    await transacao.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
        public virtual async Task<bool> Atualizar(object objeto, bool writeToJson = true, bool sendToServer = true)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.UpdateAsync(objeto);

                    if (writeToJson)
                    {
                        var databaseLogFile = WriteDatabaseLogFile("UPDATE", objeto);

                        if (sendToServer)
                            SendDatabaseLogFileToServer(databaseLogFile, objeto.GetType());
                    }

                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO ATUALIZAR >>> " + ex.Message);
                }

                return false;
            }
        }
        public virtual async Task<bool> Deletar(object objeto, bool writeToJson = true, bool sendToServer = true)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.DeleteAsync(objeto);
                    if (writeToJson)
                    {
                        var databaseLogFile = WriteDatabaseLogFile("DELETE", objeto);

                        if (sendToServer)
                            SendDatabaseLogFileToServer(databaseLogFile, objeto.GetType());
                    }
                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO DELETAR >>> " + ex.Message);
                }

                return false;
            }
        }
        public virtual async Task<bool> Deletar<E>(IList<E> objetos, bool writeToJson = true, bool sendToServer = true) where E : class, IModel
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        await session.DeleteAsync(e);
                    }

                    if (writeToJson)
                    {
                        foreach (E e in objetos)
                        {
                            DatabaseLogFile<E> databaseLogFile = SincronizacaoViewModel.WriteDatabaseLogFile("DELETE", e);

                            if (sendToServer)
                                SincronizacaoViewModel.SendDatabaseLogFileToServer(databaseLogFile);
                        }
                    }

                    await transacao.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO DELETAR >>> " + ex.Message);
                }

                return false;
            }
        }
        public virtual async Task<IList<E>> Listar<E>() where E : class, IModel
        {
            try
            {
                var criteria = CriarCriteria<E>();
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                return await criteria.ListAsync<E>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public virtual async Task<IList<E>> Listar<E>(ICriteria criteria) where E : class, IModel
        {
            try
            {
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                return await criteria.ListAsync<E>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public abstract Task<object> ListarPorId(object id);
        public ICriteria CriarCriteria<E>() where E : class, IModel
        {
            return session.CreateCriteria<E>();
        }

        /// <summary>
        /// Escreve DatabaseLogFile em Json usando Reflection
        /// </summary>
        /// <param name="operacao">Tipo de Operação no Banco de Dados</param>
        /// <param name="objeto">Objeto de Entidade Sendo Usada</param>
        /// <returns>Método Para Escrever DatabaseLogFile em Json</returns>
        private object WriteDatabaseLogFile(string operacao, object objeto)
        {
            Type type = typeof(SincronizacaoViewModel);
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            var method = methods.Where(w =>
                w.Name.Equals("WriteDatabaseLogFile")
                && w.GetParameters().Length == 2
                && w.GetParameters()[0].ParameterType.Name.Equals("String")
                && w.GetParameters()[1].ParameterType.Name.Equals("E"))
                .SingleOrDefault();

            if (method != null)
                return method.MakeGenericMethod(objeto.GetType()).Invoke(null, new object[] { operacao, objeto });

            throw new NullReferenceException("Método Não Foi Encontrado");
        }

        private object SendDatabaseLogFileToServer(object databaseLogFile, Type tipoEntidade)
        {
            try
            {
                Type type = typeof(SincronizacaoViewModel);
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
                var method = methods.Where(w =>
                    w.Name.Equals("SendDatabaseLogFileToServer"))
                    .SingleOrDefault();

                if (method != null)
                    return method.MakeGenericMethod(tipoEntidade).Invoke(null, new object[] { databaseLogFile });

                throw new NullReferenceException("Método Não Foi Encontrado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DAO SendDatabaseLogFileToServer | Não Possível Enviar Log ao Servidor | {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}
