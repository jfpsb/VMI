using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using System;
using System.Collections.Generic;

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

        private static Dictionary<string, ISession> _sessions = new Dictionary<string, ISession>();

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

        public static ISession GetSession(string formId)
        {
            if (MySessionFactory == null)
            {
                MySessionFactory = BuildSessionFactory();
            }

            if (_sessions.ContainsKey(formId))
            {
                return _sessions[formId];
            }

            ISession _session = MySessionFactory.WithOptions().Interceptor(new SQLInterceptor()).OpenSession();

            _sessions.Add(formId, _session);

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

        public static void FechaSession(string formId)
        {
            if (_sessions.ContainsKey(formId))
            {
                _sessions[formId]?.Dispose();
                _sessions.Remove(formId);
            }
            Console.WriteLine($"Sessão Fechada: {formId}");
        }
    }
}
