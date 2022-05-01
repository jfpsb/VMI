using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using System;
using VandaModaIntimaWpf.Model;

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
            MainConfiguration.Configure("hibernateLocal.cfg.xml");
            MainConfiguration.SetListener(ListenerType.PreUpdate, new NHibernatePreEventListener());
            MainConfiguration.SetListener(ListenerType.PreInsert, new NHibernatePreEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new AdiantamentoPostEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new CompraDeFornecedorPostEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new ContagemPostEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new EntradaDeMercadoriaPostEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new FuncionarioPostEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new GradePostEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new LojaPostEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new ProdutoPostEventListener());
            MainConfiguration.SetListener(ListenerType.PostUpdate, new ProdutoGradePostEventListener());
            return MainConfiguration.BuildSessionFactory();
        }

        public static ISession GetSession()
        {
            if (MainSessionFactory == null)
            {
                MainSessionFactory = BuildSessionFactory();
            }

            ISession _session = MainSessionFactory.OpenSession();

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
