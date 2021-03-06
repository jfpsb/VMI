﻿using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Funcionario
{
    /// <summary>
    /// Interaction logic for CadastrarFuncionario.xaml
    /// </summary>
    public partial class SalvarFuncionario : Window
    {
        public SalvarFuncionario()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.Funcionario>)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCpf.Focus();
        }
    }
}
