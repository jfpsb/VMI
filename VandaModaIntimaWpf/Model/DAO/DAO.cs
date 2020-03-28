using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public virtual async Task<bool> Inserir(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.SaveAsync(objeto);
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
        public virtual async Task<bool> Inserir<E>(IList<E> objetos) where E : class, IModel
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        await session.SaveOrUpdateAsync(e);
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
        public virtual async Task<bool> InserirOuAtualizar(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.SaveOrUpdateAsync(objeto);
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
        public virtual async Task<bool> InserirOuAtualizar<E>(IList<E> objetos) where E : class, IModel
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        await session.SaveOrUpdateAsync(e);
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
        public virtual async Task<bool> Atualizar(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.UpdateAsync(objeto);
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
        public virtual async Task<bool> Deletar(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.DeleteAsync(objeto);
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
        public virtual async Task<bool> Deletar<E>(IList<E> objetos) where E : class, IModel
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        await session.DeleteAsync(e);
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
        public abstract int GetMaxId();
        public ICriteria CriarCriteria<E>() where E : class, IModel
        {
            return session.CreateCriteria<E>();
        }
    }
}
