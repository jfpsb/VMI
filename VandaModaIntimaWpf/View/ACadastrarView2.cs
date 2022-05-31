using System.Windows;
using System.Windows.Forms;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View
{
    public class ACadastrarView2 : Window, IOpenFileDialog
    {
        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.FileName != string.Empty)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        protected void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = (DataContext as ICadastrarVM).ResultadoSalvar();
        }
    }
}
