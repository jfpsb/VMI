﻿using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAO<E> where E : class, IModel
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
                    var result = await session.SaveAsync(objeto);
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
        public virtual async Task<bool> Inserir(IList<E> objetos)
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
        public virtual async Task<bool> InserirOuAtualizar(IList<E> objetos)
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
        public virtual async Task<bool> Merge(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.MergeAsync(objeto);
                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO FAZER MERGE >>> " + ex.Message);
                    return false;
                }
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
        public virtual async Task<bool> Deletar(IList<E> objetos)
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
        public virtual async Task<IList<E>> Listar()
        {
            try
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    var criteria = CriarCriteria();
                    criteria.SetCacheable(true);
                    criteria.SetCacheMode(CacheMode.Normal);
                    var results = await criteria.ListAsync<E>();
                    await tx.CommitAsync();
                    return results;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public virtual async Task<IList<E>> Listar(ICriteria criteria)
        {
            try
            {
                using (ITransaction tx = session.BeginTransaction())
                {
                    criteria.SetCacheable(true);
                    criteria.SetCacheMode(CacheMode.Normal);
                    var results = await criteria.ListAsync<E>();
                    await tx.CommitAsync();
                    return results;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public async Task<E> ListarPorId(object id)
        {
            return await session.GetAsync<E>(id);
        }
        public async Task<long> RetornaMaiorValor(string idProperty)
        {
            var criteria = session.CreateCriteria<E>();
            criteria.SetProjection(Projections.Max(idProperty));
            return await criteria.UniqueResultAsync<long>();
        }
        public ICriteria CriarCriteria()
        {
            return session.CreateCriteria<E>();
        }

        public ICriteria CriarCriteria(string alias)
        {
            return session.CreateCriteria<E>(alias);
        }
    }
}
