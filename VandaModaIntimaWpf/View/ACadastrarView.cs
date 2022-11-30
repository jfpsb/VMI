using System.Windows;
using System.Windows.Forms;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View
{
    public class ACadastrarView : Window, IOpenFileDialog
    {
        public ACadastrarView()
        {
            Closing += Window_Closing;
        }
        public string OpenFileDialog(string filtro = "")
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (filtro.Length > 0) openFileDialog.Filter = filtro;

            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != string.Empty)
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        protected void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = (DataContext as IDialogResult).ResultadoDialog();
        }
    }
}
