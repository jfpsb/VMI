using NHibernate;
using System;
using System.Collections.Generic;
using VandaModaIntima.BancoDeDados.ConnectionFactory;

namespace VandaModaIntima.Model.DAO
{
    public class DAOMySQL<T> : IDAO<T> where T : class
    {
        protected ISession session;
        private bool isDisposed = false;

        public DAOMySQL()
        {
            session = SessionProvider.GetMySession();
        }

        public bool Atualizar(T objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    session.Update(objeto);
                    transacao.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    Console.WriteLine("ERRO AO ATUALIZAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public bool Deletar(T objeto)
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
                    transacao.Rollback();
                    Console.WriteLine("ERRO AO DELETAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public bool Inserir(T objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    session.Save(objeto);
                    transacao.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    Console.WriteLine("ERRO AO INSERIR >>> " + ex.Message);
                }

                return false;
            }
        }

        public bool InserirOuAtualizar(T objeto)
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
                    transacao.Rollback();
                    Console.WriteLine("ERRO AO INSERIR OU ATUALIZAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public IList<T> Listar(ICriteria criteria)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    criteria.SetCacheable(true);
                    criteria.SetCacheMode(CacheMode.Normal);

                    return criteria.List<T>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO LISTAR >>> " + ex.Message);
                }

                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            if (disposing)
            {
                SessionProvider.FechaSession(session);
            }

            isDisposed = true;
        }

        public ICriteria CriarCriteria()
        {
            return session.CreateCriteria<T>();
        }

        ~DAOMySQL()
        {
            Dispose(false);
        }
    }
}
