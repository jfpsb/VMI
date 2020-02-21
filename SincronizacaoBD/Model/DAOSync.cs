using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace SincronizacaoBD.Model
{
    public class DAOSync<E> where E : class
    {
        public void InserirOuAtualizar(E objeto)
        {
            using (ISession session = SessionSyncProvider.GetSession())
            {
                using (var transacao = session.BeginTransaction())
                {
                    session.SaveOrUpdate(objeto);
                    transacao.Commit();
                }
            }
        }
        public void InserirOuAtualizar(IList<E> objetos)
        {
            using (ISession session = SessionSyncProvider.GetSession())
            {
                using (var transacao = session.BeginTransaction())
                {
                    foreach (E e in objetos)
                    {
                        session.SaveOrUpdate(e);
                    }

                    transacao.Commit();
                }
            }
        }
        public virtual void Deletar(E objeto)
        {
            using (ISession session = SessionSyncProvider.GetSession())
            {
                using (var transacao = session.BeginTransaction())
                {
                    session.Delete(objeto);
                    transacao.Commit();
                }
            }
        }

        public virtual void Deletar(IList<E> objetos)
        {
            using (ISession session = SessionSyncProvider.GetSession())
            {
                using (var transacao = session.BeginTransaction())
                {
                    foreach (E e in objetos)
                    {
                        session.Delete(e);
                    }

                    transacao.Commit();
                }
            }
        }
        public IList<E> ListarLastUpdate(DateTime lastUpdate, params string[] fetchPaths)
        {
            using (ISession session = SessionSyncProvider.GetSession())
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
        }
        public IList<E> Listar()
        {
            using (ISession session = SessionSyncProvider.GetSession())
            {
                var criteria = session.CreateCriteria<E>();
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);

                return criteria.List<E>();
            }
        }
    }
}
