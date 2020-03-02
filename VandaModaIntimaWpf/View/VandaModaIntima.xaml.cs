using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();

            //Sincronizacao.OperacoesDatabaseLogFile<IModel>.ResetaLogs();

            SessionProvider.MySessionFactory = SessionProvider.BuildSessionFactory();

            SincronizacaoBD.SyncMainWindow syncMainWindow = new SincronizacaoBD.SyncMainWindow();
            syncMainWindow.Show();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionProvider.FechaConexoes();
        }

        private void TelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
