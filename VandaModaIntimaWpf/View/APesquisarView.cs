﻿using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View
{
    public partial class APesquisarView : Window, IMessageable, IOpenFileDialog, ICloseable
    {
        public void Pesquisar_Closing(object sender, CancelEventArgs e)
        {
            if (((IPesquisarViewModel)DataContext).IsThreadLocked())
            {
                e.Cancel = true;
            }
            else
            {
                //Fecha sessao
                ((IPesquisarViewModel)DataContext).DisposeSession();
            }
        }

        public void MensagemDeAviso(string mensagem)
        {
            MessageBox.Show(mensagem, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void MensagemDeErro(string mensagem)
        {
            MessageBox.Show(mensagem, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public MessageBoxResult MensagemSimOuNao(string mensagem, string caption)
        {
            return MessageBox.Show(mensagem, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
    }
}
