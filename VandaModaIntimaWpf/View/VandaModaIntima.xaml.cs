using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;

namespace VandaModaIntimaWpf.View
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionProvider.FechaSessionFactory();
        }
    }
}
