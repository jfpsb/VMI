using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SincronizacaoBD.Model
{
    public class DAOSync<E> where E : class
    {
        private ISession session;
        public DAOSync(ISession session)
        {
            this.session = session;
        }

        public bool InserirOuAtualizar(E objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    session.SaveOrUpdate(objeto);
                    transacao.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO INSERIR OU ATUALIZAR SYNC >>> " + ex.Message);
                }

                return false;
            }
        }
        public bool InserirOuAtualizar(IList<E> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        session.SaveOrUpdate(e);
                    }

                    transacao.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO INSERIR OU ATUALIZAR LISTA SYNC >>> " + ex.Message);
                }

                return false;
            }
        }
        public virtual bool Deletar(E objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    session.Delete(objeto);
                    transacao.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO DELETAR SYNC >>> " + ex.Message);
                }

                return false;
            }
        }

        public virtual bool Deletar(IList<E> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        session.Delete(e);
                    }

                    transacao.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO DELETAR LISTA SYNC >>> " + ex.Message);
                }

                return false;
            }
        }
        public IList<E> ListarLastUpdate(DateTime lastUpdate, params string[] fetchPaths)
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

                return criteria.List<E>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }
}
