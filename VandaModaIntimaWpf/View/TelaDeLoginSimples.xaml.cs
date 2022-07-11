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

namespace VandaModaIntimaWpf.View
{
    /// <summary>
    /// Interaction logic for TelaDeLoginSimples.xaml
    /// </summary>
    public partial class TelaDeLoginSimples : Window
    {
        public TelaDeLoginSimples()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).Senha = ((PasswordBox)sender).Password;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((dynamic)DataContext).Resultado;
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
