using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

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
        public static Configuration MyConfigurationSync;

        /// <summary>
        /// Guarda a Session Factory criada para uso em DAO.
        /// </summary>        
        public static ISessionFactory MySessionFactory = null;
        public static ISessionFactory MySessionFactorySync = null;

        /// <summary>
        /// Método responsável pela criação da Session Factory.
        /// </summary>
        /// <returns>myConfiguration.BuildSessionFactory()</returns>
        public static ISessionFactory BuildSessionFactory()
        {
            MyConfiguration = new Configuration();
            MyConfiguration.Configure("hibernateLocal.cfg.xml");
            return MyConfiguration.BuildSessionFactory();
        }

        public static ISessionFactory BuildSessionFactorySync()
        {
            MyConfigurationSync = new Configuration();
            MyConfigurationSync.Configure("hibernateSync.cfg.xml");
            return MyConfigurationSync.BuildSessionFactory();
        }

        public static ISession GetSession(string formId)
        {
            if (MySessionFactory == null)
            {
                MySessionFactory = BuildSessionFactory();
            }

            ISession _session = MySessionFactory.OpenSession();

            return _session;
        }

        public static ISession GetSessionSync()
        {
            if (MySessionFactorySync == null)
            {
                MySessionFactorySync = BuildSessionFactorySync();
            }

            ISession _session = MySessionFactorySync.OpenSession();

            return _session;
        }

        public static void FechaConexoes()
        {
            if (MySessionFactory != null && !MySessionFactory.IsClosed)
            {
                MySessionFactory.Close();
                Console.WriteLine("MySessionFactory fechada");
            }
        }

        public static void FechaConexoesSync()
        {
            if (MySessionFactorySync != null && !MySessionFactorySync.IsClosed)
            {
                MySessionFactorySync.Close();
                Console.WriteLine("MySessionFactorySync fechada");
            }
        }

        public static void FechaSession(ISession session)
        {
            session?.Dispose();
            Console.WriteLine("Sessão Fechada");
        }
    }
}
