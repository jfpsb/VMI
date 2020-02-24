using NHibernate;
using System.Collections.Generic;

namespace SincronizacaoBD.Model
{
    public class DAOSync
    {
        public void InserirOuAtualizar(object objeto)
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
        public bool InserirOuAtualizar(IList<object> objetos)
        {
            if (objetos.Count == 0)
                return false;

            using (ISession session = SessionSyncProvider.GetSession())
            {
                using (var transacao = session.BeginTransaction())
                {
                    foreach (object e in objetos)
                    {
                        session.SaveOrUpdate(e);
                    }

                    transacao.Commit();
                    return true;
                }
            }
        }
        public void Deletar(object objeto)
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

        public bool Deletar(IList<object> objetos)
        {
            if (objetos.Count == 0)
                return false;

            using (ISession session = SessionSyncProvider.GetSession())
            {
                using (var transacao = session.BeginTransaction())
                {
                    foreach (object e in objetos)
                    {
                        session.Delete(e);
                    }

                    transacao.Commit();
                    return true;
                }
            }
        }
    }
}
