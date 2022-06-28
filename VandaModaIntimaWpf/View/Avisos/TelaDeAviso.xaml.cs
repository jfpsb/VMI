using System.Windows;
using VandaModaIntimaWpf.ViewModel.Avisos;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.View.Avisos
{
    /// <summary>
    /// Interaction logic for TelaDeAviso.xaml
    /// </summary>
    public partial class TelaDeAviso : Window
    {
        public TelaDeAviso()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as IViewModelClosing).OnClosing();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if ((DataContext as TelaDeAvisoVM).ItensAvisos.Count == 0)
                Close();
        }
    }
}
