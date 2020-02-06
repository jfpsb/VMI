using SincronizacaoBD;
using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;

namespace VandaModaIntimaWpf
{
    public partial class VandaModaIntima : Window
    {
        private MainWindow mainWindow;
        public VandaModaIntima()
        {
            InitializeComponent();

            SessionProvider.MySessionFactory = SessionProvider.BuildSessionFactory();

            mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow?.Close();
            SessionProvider.FechaConexoes();
        }
    }
}
