using System.Windows;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    /// <summary>
    /// Interaction logic for ConfirmarConsolidacaoPontosEletronicos.xaml
    /// </summary>
    public partial class ConfirmarConsolidacaoPontosEletronicos : Window
    {
        public ConfirmarConsolidacaoPontosEletronicos()
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
