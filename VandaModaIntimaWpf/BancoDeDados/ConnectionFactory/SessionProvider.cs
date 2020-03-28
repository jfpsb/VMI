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
        public static Configuration MainConfiguration;
        public static Configuration SecondaryConfiguration;

        /// <summary>
        /// Guarda a Session Factory criada para uso em DAO.
        /// </summary>        
        public static ISessionFactory MainSessionFactory = null;
        public static ISessionFactory SecondarySessionFactory = null;

        private static Dictionary<string, ISession> _mainSessions = new Dictionary<string, ISession>();
        private static Dictionary<string, ISession> _secondarySessions = new Dictionary<string, ISession>();

        /// <summary>
        /// Método responsável pela criação da Session Factory.
        /// </summary>
        /// <returns>myConfiguration.BuildSessionFactory()</returns>
        public static ISessionFactory BuildMainSessionFactory()
        {
            MainConfiguration = new Configuration();
            MainConfiguration.Configure("mainHibernate.cfg.xml");
            return MainConfiguration.BuildSessionFactory();
        }

        public static ISessionFactory BuildSecondarySessionFactory()
        {
            MainConfiguration = new Configuration();
            MainConfiguration.Configure("secondaryHibernate.cfg.xml");
            return MainConfiguration.BuildSessionFactory();
        }

        public static ISession GetMainSession(string formId)
        {
            if (MainSessionFactory == null)
            {
                MainSessionFactory = BuildMainSessionFactory();
            }

            if (_mainSessions.ContainsKey(formId))
            {
                return _mainSessions[formId];
            }

            ISession _session = MainSessionFactory.OpenSession();

            _mainSessions.Add(formId, _session);

            return _session;
        }

        public static ISession GetSecondarySession(string formId)
        {
            if (SecondarySessionFactory == null)
            {
                SecondarySessionFactory = BuildSecondarySessionFactory();
            }

            if (_secondarySessions.ContainsKey(formId))
            {
                return _secondarySessions[formId];
            }

            ISession _session = SecondarySessionFactory.OpenSession();

            _secondarySessions.Add(formId, _session);

            return _session;
        }

        public static void FechaMainConexoes()
        {
            if (MainSessionFactory != null && !MainSessionFactory.IsClosed)
            {
                MainSessionFactory.Close();
                Console.WriteLine("MainSessionFactory fechada");
            }
        }

        public static void FechaSecondaryConexoes()
        {
            if (SecondarySessionFactory != null && !SecondarySessionFactory.IsClosed)
            {
                SecondarySessionFactory.Close();
                Console.WriteLine("SecondarySessionFactory fechada");
            }
        }

        public static void FechaMainSession(string formId)
        {
            if (_mainSessions.ContainsKey(formId))
            {
                _mainSessions[formId]?.Dispose();
                _mainSessions.Remove(formId);
            }
            Console.WriteLine($"Sessão Principal Fechada: {formId}");
        }

        public static void FechaSecondarySession(string formId)
        {
            if (_secondarySessions.ContainsKey(formId))
            {
                _secondarySessions[formId]?.Dispose();
                _secondarySessions.Remove(formId);
            }
            Console.WriteLine($"Sessão Secundária Fechada: {formId}");
        }
    }
}
