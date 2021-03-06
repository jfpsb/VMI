﻿using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Marca
{
    /// <summary>
    /// Interaction logic for CadastrarMarca.xaml
    /// </summary>
    public partial class CadastrarMarca : Window
    {
        public CadastrarMarca()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.Marca>)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtMarca.Focus();
        }
    }
}
