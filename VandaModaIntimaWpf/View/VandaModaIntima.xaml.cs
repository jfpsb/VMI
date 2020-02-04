using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.BancoDeDados.Sincronizacao;

namespace VandaModaIntimaWpf
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();
            SessionProvider.MySessionFactory = SessionProvider.BuildSessionFactory();
            SessionProvider.MySessionFactorySync = SessionProvider.BuildSessionFactorySync();
            ChamaSincronizacao();
        }

        private async void ChamaSincronizacao()
        {
            await SincronizacaoRemota.Sincronizar();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionProvider.FechaConexoes();
            SessionProvider.FechaConexoesSync();
        }
    }
}
