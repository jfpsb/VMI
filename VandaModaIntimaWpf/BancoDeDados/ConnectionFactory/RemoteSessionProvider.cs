using NHibernate;
using NHibernate.Cfg;
using System;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    /// <summary>
    /// Classe estática responsável pelas Sessions necessárias para o uso de banco de dados com NHibernate.
    /// </summary>
    public static class RemoteSessionProvider
    {
        /// <summary>
        /// Variável que guardará a configuração necessária contida em hibernate.cfg.xml.
        /// </summary>
        public static Configuration MainConfiguration;

        /// <summary>
        /// Guarda a Session Factory criada para uso em DAO.
        /// </summary>        
        public static ISessionFactory MainSessionFactory = null;

        /// <summary>
        /// Método responsável pela criação da Session Factory.
        /// </summary>
        /// <returns>myConfiguration.BuildSessionFactory()</returns>
        public static ISessionFactory BuildSessionFactory()
        {
            MainConfiguration = new Configuration();
            MainConfiguration.Configure("hibernateRemoto.cfg.xml");
            return MainConfiguration.BuildSessionFactory();
        }

        public static ISession GetSession()
        {
            if (MainSessionFactory == null)
            {
                MainSessionFactory = BuildSessionFactory();
            }

            ISession _session = MainSessionFactory.WithOptions().Interceptor(new LocalInterceptor()).OpenSession();

            return _session;
        }

        public static void FechaSessionFactory()
        {
            if (MainSessionFactory != null && !MainSessionFactory.IsClosed)
            {
                MainSessionFactory.Close();
                Console.WriteLine("MainSessionFactory fechada");
            }
        }

        public static void FechaSession(ISession session)
        {
            if (session.IsOpen)
            {
                session?.Dispose();
            }
            Console.WriteLine($"Sessão Fechada");
        }
    }
}
