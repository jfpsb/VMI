using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;

namespace VandaModaIntimaWpf
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();
            LocalSessionProvider.MainSessionFactory = LocalSessionProvider.BuildSessionFactory();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LocalSessionProvider.FechaSessionFactory();
        }
    }
}
