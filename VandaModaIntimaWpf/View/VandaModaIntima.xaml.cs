using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;

namespace VandaModaIntimaWpf
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();
            SessionProvider.BuildSessionFactory();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
