using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace VandaModaIntimaWpf.View
{
    public partial class APesquisarView : Window, IOpenFileDialog, ISaveFileDialog, IFolderBrowserDialog, ICloseable
    {
        public void Pesquisar_Closing(object sender, CancelEventArgs e)
        {
            if (((IPesquisarVM)DataContext).IsThreadLocked())
            {
                e.Cancel = true;
            }
            else
            {
                //Fecha sessao
                ((IPesquisarVM)DataContext).DisposeSession();
            }
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

        public string OpenSaveFileDialog(string titulo, string filtros)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = titulo,
                Filter = filtros
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }

        public string OpenFolderBrowserDialog()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Selecione A Pasta";

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }

            return null;
        }
    }
}
