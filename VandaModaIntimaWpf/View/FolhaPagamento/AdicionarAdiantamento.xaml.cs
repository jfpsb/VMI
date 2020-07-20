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

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for AdicionarAdiantamento.xaml
    /// </summary>
    public partial class AdicionarAdiantamento : Window
    {
        public AdicionarAdiantamento()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TxtValor.Focus();
        }
    }
}
