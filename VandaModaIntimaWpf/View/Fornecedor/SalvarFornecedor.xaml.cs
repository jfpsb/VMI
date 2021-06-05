﻿using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Fornecedor
{
    /// <summary>
    /// Interaction logic for CadastrarFornecedorManualmente.xaml
    /// </summary>
    public partial class SalvarFornecedor : Window
    {
        public SalvarFornecedor()
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

        private void TxtCnpj_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TxtCnpj.CaretIndex = TxtCnpj.Text.Length;
        }
    }
}