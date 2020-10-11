﻿using System.Windows;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Contagem
{
    /// <summary>
    /// Interaction logic for CadastrarContagem.xaml
    /// </summary>
    public partial class CadastrarContagem : Window
    {
        public CadastrarContagem()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = ((ACadastrarViewModel<Model.Contagem>)DataContext).ResultadoSalvar();
            if (result != null)
                DialogResult = true;
            else
                DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CmbLoja.Focus();
        }
    }
}
