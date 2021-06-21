using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for GerenciarParcelas.xaml
    /// </summary>
    public partial class GerenciarParcelas : Window
    {
        public GerenciarParcelas()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = (DataContext as IDialogResult).ResultadoDialog();
        }
    }
}
