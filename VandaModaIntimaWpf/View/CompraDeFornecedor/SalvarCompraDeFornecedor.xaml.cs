using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.CompraDeFornecedor
{
    /// <summary>
    /// Interaction logic for SalvarCompraDeFornecedor.xaml
    /// </summary>
    public partial class SalvarCompraDeFornecedor : Window
    {
        public SalvarCompraDeFornecedor()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.CompraDeFornecedor>)DataContext).ResultadoSalvar();
        }
    }
}
