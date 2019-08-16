using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    /// <summary>
    /// Classe estática responsável pelas Sessions necessárias para o uso de banco de dados com NHibernate.
    /// </summary>
    public static class SessionProvider
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
            MyConfiguration.Configure();
            return MyConfiguration.BuildSessionFactory();
        }

        public static ISession GetSession()
        {
            ISession session;

            if (MySessionFactory == null)
            {
                MySessionFactory = BuildSessionFactory();
            }

            if (CurrentSessionContext.HasBind(MySessionFactory))
            {
                session = MySessionFactory.GetCurrentSession();
            }
            else
            {
                session = MySessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }

            return session;
        }

        public static void FechaConexoes()
        {
            if (MySessionFactory != null && !MySessionFactory.IsClosed)
                MySessionFactory.Close();
        }

        public static void FechaSession()
        {
            ISession s = CurrentSessionContext.Unbind(MySessionFactory);
            s.Dispose();
            Console.WriteLine("Sessão Fechada");
        }
    }
}
