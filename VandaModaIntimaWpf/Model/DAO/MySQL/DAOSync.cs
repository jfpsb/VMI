using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOSync<E> where E : class, IModel
    {
        private ISession session;
        public DAOSync(ISession session)
        {
            this.session = session;
        }

        public async Task<bool> InserirOuAtualizar(E objeto)
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
                    Console.WriteLine("ERRO AO INSERIR OU ATUALIZAR SYNC >>> " + ex.Message);
                }

                return false;
            }
        }
        public async Task<bool> InserirOuAtualizar(IList<E> objetos)
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
                    Console.WriteLine("ERRO AO INSERIR OU ATUALIZAR LISTA SYNC >>> " + ex.Message);
                }

                return false;
            }
        }
        public virtual async Task<bool> Deletar(E objeto)
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
                    Console.WriteLine("ERRO AO DELETAR SYNC >>> " + ex.Message);
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
                    Console.WriteLine("ERRO AO DELETAR LISTA SYNC >>> " + ex.Message);
                }

                return false;
            }
        }
        public async Task<IList<E>> ListarLastUpdate(DateTime lastUpdate, params string[] fetchPaths)
        {
            try
            {
                var criteria = session.CreateCriteria<E>();

                criteria.Add(Restrictions.Ge("LastUpdate", lastUpdate));

                foreach (string fetchPath in fetchPaths)
                {
                    criteria.Fetch(fetchPath);
                }

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
    }
}
