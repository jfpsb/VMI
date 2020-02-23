using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Sincronizacao;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public abstract class DAOMySQL<T> : IDAO<T> where T : class, IModel, ICloneable
    {
        protected ISession session;
        public DAOMySQL(ISession session)
        {
            this.session = session;
        }

        public async Task<bool> Atualizar(T objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.UpdateAsync(objeto);
                    await transacao.CommitAsync();
                    OperacoesDatabaseLogFile<T>.EscreverJson("UPDATE", objeto);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO ATUALIZAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public virtual async Task<bool> Deletar(T objeto)
        {
            string jsonFileName = $"{typeof(T).Name} {objeto.GetIdentifier()}.json";

            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    OperacoesDatabaseLogFile<T>.EscreverJson("DELETE", objeto);
                    await session.DeleteAsync(objeto);
                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    OperacoesDatabaseLogFile<T>.DeletaArquivo(jsonFileName);
                    Console.WriteLine("ERRO AO DELETAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public virtual async Task<bool> Deletar(IList<T> objetos)
        {
            List<string> jsonFileNames = new List<string>();

            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (T t in objetos)
                    {
                        jsonFileNames.Add($"{typeof(T).Name} {t.GetIdentifier()}.json");
                        OperacoesDatabaseLogFile<T>.EscreverJson("DELETE", t);
                        await session.DeleteAsync(t);
                    }

                    await transacao.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    foreach (string jsonFileName in jsonFileNames)
                    {
                        OperacoesDatabaseLogFile<T>.DeletaArquivo(jsonFileName);
                    }
                    Console.WriteLine("ERRO AO DELETAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public async Task<bool> Inserir(T objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.SaveAsync(objeto);
                    await transacao.CommitAsync();
                    OperacoesDatabaseLogFile<T>.EscreverJson("INSERT", objeto);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO INSERIR >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }

        public virtual async Task<bool> Inserir(IList<T> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (T t in objetos)
                    {
                        //Testa se a id do objeto sendo adicionado já existe no bd
                        var o = await session.GetAsync<T>(t.GetIdentifier());
                        if (o == null)
                        {
                            await session.SaveAsync(t);
                        }
                        else
                        {
                            session.Evict(o);
                            await session.UpdateAsync(t);
                        }
                    }

                    await transacao.CommitAsync();

                    foreach (T t in objetos)
                    {
                        //Testa se a id do objeto sendo adicionado já existe no bd
                        var o = await session.GetAsync<T>(t.GetIdentifier());
                        if (o == null)
                        {
                            OperacoesDatabaseLogFile<T>.EscreverJson("INSERT", t);
                        }
                        else
                        {
                            OperacoesDatabaseLogFile<T>.EscreverJson("UPDATE", t);
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
        public virtual async Task<IList<T>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                return await criteria.ListAsync<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public async Task<IList<T>> Listar(ICriteria criteria)
        {
            try
            {
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                return await criteria.ListAsync<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public ICriteria CriarCriteria()
        {
            return session.CreateCriteria<T>();
        }

        public abstract Task<T> ListarPorId(params object[] id);
    }
}
