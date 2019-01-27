using NHibernate;
using NHibernate.Cfg;
using System;
using System.Windows.Forms;

namespace VandaModaIntima.BancoDeDados.ConnectionFactory
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

            //try
            //{
                
            //}
            //catch (InvalidProxyTypeException ite)
            //{
            //    MessageBox.Show("Erro ao conectar. Cheque sua conexão com a internet." + ite.Message + "\n\n" + ite.StackTrace, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return null;
            //}
            //catch (MappingException me)
            //{
            //    MessageBox.Show("Erro de mapa\n\n" + me.Message + "\n\n" + me.Data + "\n\n" + me.StackTrace + "\n\n" + me.InnerException?.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return null;
            //}
        }
        /// <summary>
        /// Retorna a Session Factory para uso em classe DAO.
        /// </summary>
        /// <returns>mySessionFactory</returns>
        public static ISessionFactory GetSessionFactory()
        {
            return MySessionFactory;
        }

        public static ISession GetMySession()
        {
            ISession session;

            if (MySessionFactory == null)
            {
                MySessionFactory = BuildSessionFactory();
            }

            if (MySessionFactory != null)
            {
                session = MySessionFactory.OpenSession();
                return session;
                //try
                //{
                    
                //}
                //catch (Exception e)
                //{
                //    MessageBox.Show(e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }

            return null;
        }

        public static void FechaConexoes()
        {
            if (MySessionFactory != null && !MySessionFactory.IsClosed)
                MySessionFactory.Close();
        }

        public static void FechaSession(ISession session)
        {
            if (session != null && session.IsOpen)
            {
                session.Close();
                Console.WriteLine("Sessão Fechada");
            }
        }
    }
}
