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

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    /// <summary>
    /// Interaction logic for ConsolidarPontosEletronicos.xaml
    /// </summary>
    public partial class ConsolidarPontosEletronicos : Window
    {
        public ConsolidarPontosEletronicos()
        {
            InitializeComponent();
        }

        private double pos = 0.0;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                ScrollViewer.ScrollToHorizontalOffset(++pos);
            }

            if (e.Key == Key.Right)
            {
                ScrollViewer.ScrollToHorizontalOffset(++pos);
            }
        }

        private void ScrollViewer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (pos > 0)
                    pos -= 220.0;
                ScrollViewer.ScrollToHorizontalOffset(pos);
            }

            if (e.Key == Key.Right)
            {
                if (pos < (ItemsControl.Items.Count * 220.0))
                    pos += 220.0;
                ScrollViewer.ScrollToHorizontalOffset(pos);
            }
        }
    }
}
