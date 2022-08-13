using System.Windows;

namespace VandaModaIntimaWpf.View.Pix
{
    /// <summary>
    /// Interaction logic for PagamentoPix.xaml
    /// </summary>
    public partial class PagamentoPix : Window
    {
        public PagamentoPix()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext != null)
                (DataContext as dynamic).FechaSession();
        }
    }
}
