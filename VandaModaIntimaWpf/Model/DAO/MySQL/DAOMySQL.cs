using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public abstract class DAOMySQL<T> : IDAO<T> where T : class, IModel
    {
        protected ISession session;

        public DAOMySQL(ISession session)
        {
            this.session = session;
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

        public bool Deletar(IList<T> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (T t in objetos)
                    {
                        session.Delete(t);
                    }

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
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }

        public bool Inserir(IList<T> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (T t in objetos)
                    {
                        //Testa se a id do objeto sendo adicionado já existe no bd
                        var o = session.Get<T>(t.GetId());
                        if (o == null)
                            session.Save(t);
                    }

                    transacao.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.InnerException.Message);
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
        public IList<T> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                return criteria.List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public IList<T> Listar(ICriteria criteria)
        {
            try
            {
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                return criteria.List<T>();
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

        public abstract T ListarPorId(object id);
    }
}
