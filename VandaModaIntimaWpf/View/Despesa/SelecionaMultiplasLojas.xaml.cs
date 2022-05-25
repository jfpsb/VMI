using System.Windows;
using System.Windows.Controls;
using VandaModaIntimaWpf.ViewModel.Interfaces;

namespace VandaModaIntimaWpf.View.Despesa
{
    /// <summary>
    /// Interaction logic for SelecionaMultiplasLojas.xaml
    /// </summary>
    public partial class SelecionaMultiplasLojas : UserControl
    {
        private Window window;
        public SelecionaMultiplasLojas()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Seleção de lojas para cadastro de despesas";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ResizeMode = ResizeMode.NoResize;

            if (DataContext is IRequestClose)
            {
                (DataContext as IRequestClose).RequestClose += (_, __) =>
                {
                    window.Close();
                };
            }
        }
    }
}
