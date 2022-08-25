using Microsoft.Win32;
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
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.View.Pix
{
    /// <summary>
    /// Interaction logic for ConfigurarCredenciaisPix.xaml
    /// </summary>
    public partial class ConfigurarCredenciaisPix : Window, IOpenFileDialog
    {
        public ConfigurarCredenciaisPix()
        {
            InitializeComponent();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IRequestClose)
            {
                (DataContext as IRequestClose).RequestClose += (_, _) => Close();
            }
        }
    }
}
