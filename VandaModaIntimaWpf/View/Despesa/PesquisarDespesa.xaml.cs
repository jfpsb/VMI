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
    public partial class PesquisarDespesa : APesquisarView2
    {
        public PesquisarDespesa()
        {
            InitializeComponent();
        }

        private void TelaPesquisarDespesa_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            window.Title = "Pesquisar Despesa";
            window.WindowState = WindowState.Maximized;
            window.Icon = GetIcon("/Resources/Despesas_Icon.ico");
            window.Closing += Pesquisar_Closing;
        }
    }
}
