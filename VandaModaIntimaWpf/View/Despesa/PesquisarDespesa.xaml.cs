using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.Despesa
{
    /// <summary>
    /// Interaction logic for PesquisarDespesa.xaml
    /// </summary>
    public partial class PesquisarDespesa : UserControl
    {
        private Window window;
        public PesquisarDespesa()
        {
            InitializeComponent();
        }

        private void TelaPesquisarDespesa_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Despesa";
            window.WindowState = WindowState.Maximized;
            window.Icon = new BitmapImage(new Uri("pack://application:,,,/VandaModaIntimaWpf;component/Resources/Despesas_Icon.ico"));

            window.Closing += Window_Closing;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Fecha sessao
            ((IPesquisarVM)DataContext).DisposeSession();
        }
    }
}
