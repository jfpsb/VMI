using NHibernate;
using NHibernate.Cfg;
using System;

namespace SincronizacaoBD
{
    /// <summary>
    /// Classe estática responsável pelas Sessions necessárias para o uso de banco de dados com NHibernate.
    /// </summary>
    public static class SessionSyncProvider
    {
        /// <summary>
        /// Variável que guardará a configuração necessária contida em hibernate.cfg.xml.
        /// </summary>
        public static Configuration MyConfiguration;

        /// <summary>
        /// Guarda a Session Factory criada para uso em DAO.
        /// </summary>        
        public static ISessionFactory MySessionFactory = null;

        /// <summary>
        /// Método responsável pela criação da Session Factory.
        /// </summary>
        /// <returns>myConfiguration.BuildSessionFactory()</returns>
        public static ISessionFactory BuildSessionFactory()
        {
            MyConfiguration = new Configuration();
            MyConfiguration.Configure("hibernateSync.cfg.xml");
            return MyConfiguration.BuildSessionFactory();
        }

        public static ISession GetSession()
        {
            if (MySessionFactory == null)
            {
                MySessionFactory = BuildSessionFactory();
            }

            ISession _session = MySessionFactory.OpenSession();

            return _session;
        }

        public static void FechaConexoesLocal()
        {
            if (MySessionFactory != null && !MySessionFactory.IsClosed)
            {
                MySessionFactory.Close();
                Console.WriteLine("MySessionFactory fechada");
            }
        }

        public static void FechaSession(ISession session)
        {
            session?.Dispose();
            Console.WriteLine("Sessão Fechada");
        }
    }
}
