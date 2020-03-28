﻿using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;

namespace VandaModaIntimaWpf
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();

            SessionProvider.MainSessionFactory = SessionProvider.BuildMainSessionFactory();
            SessionProvider.SecondarySessionFactory = SessionProvider.BuildSecondarySessionFactory();

            View.Sincronizacao sincronizacao = new View.Sincronizacao();
            sincronizacao.Show();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionProvider.FechaMainConexoes();
            SessionProvider.FechaSecondaryConexoes();
        }
    }
}
