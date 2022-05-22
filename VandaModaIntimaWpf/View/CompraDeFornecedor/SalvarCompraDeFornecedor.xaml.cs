using System.Windows;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View.CompraDeFornecedor
{
    /// <summary>
    /// Interaction logic for SalvarCompraDeFornecedor.xaml
    /// </summary>
    public partial class SalvarCompraDeFornecedor : Window, IOpenFileDialog
    {
        public SalvarCompraDeFornecedor()
        {
            InitializeComponent();
        }

        public string OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = ((ACadastrarViewModel<Model.CompraDeFornecedor>)DataContext).ResultadoSalvar();
        }
    }
}
