using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    /// <summary>
    /// Interaction logic for TrocarSenhaFuncionario.xaml
    /// </summary>
    public partial class TrocarSenhaFuncionario : Window
    {
        public TrocarSenhaFuncionario()
        {
            InitializeComponent();
        }

        private void PswBoxSenhaAtual_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).SenhaAtual = ((PasswordBox)sender).Password;
            }
        }

        private void PswBoxNovaSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).NovaSenha = ((PasswordBox)sender).Password;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IRequestClose)
            {
                (DataContext as IRequestClose).RequestClose += (_, __) =>
                {
                    Close();
                };
            }
        }
    }
}
