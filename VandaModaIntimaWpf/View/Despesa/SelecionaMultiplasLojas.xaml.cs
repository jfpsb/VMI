using System.Windows;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.View.Despesa
{
    /// <summary>
    /// Interaction logic for SelecionaMultiplasLojas.xaml
    /// </summary>
    public partial class SelecionaMultiplasLojas : Window
    {
        public SelecionaMultiplasLojas()
        {
            InitializeComponent();
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
