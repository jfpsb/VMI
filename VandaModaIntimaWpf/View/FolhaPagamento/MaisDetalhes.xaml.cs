using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for MaisDetalhes.xaml
    /// </summary>
    public partial class MaisDetalhes : Window
    {
        public MaisDetalhes()
        {
            InitializeComponent();
            Closing += Window_Closing;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((IDialogResult)DataContext).ResultadoDialog();
        }
    }
}
