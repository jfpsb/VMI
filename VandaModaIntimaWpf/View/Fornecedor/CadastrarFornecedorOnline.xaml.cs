﻿using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Fornecedor
{
    /// <summary>
    /// Interaction logic for CadastrarFornecedor.xaml
    /// </summary>
    public partial class CadastrarFornecedorOnline : Window
    {
        public CadastrarFornecedorOnline()
        {
            InitializeComponent();
        }

        private void TelaCadastrarFornecedor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.Fornecedor>)DataContext).ResultadoSalvar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtCnpj.Focus();
        }
    }
}
