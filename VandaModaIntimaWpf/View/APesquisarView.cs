using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View
{
    public partial class APesquisarView : Window, IOpenFileDialog, ISaveFileDialog, IFolderBrowserDialog, ICloseable
    {
        public APesquisarView()
        {
            Closing += Pesquisar_Closing;
        }
        public void Pesquisar_Closing(object sender, CancelEventArgs e)
        {
            //Fecha sessao
            ((IPesquisarVM)DataContext).DisposeSession();
        }

        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != string.Empty)
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

            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != string.Empty)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }

        public string OpenFolderBrowserDialog()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Selecione A Pasta";
            folderBrowserDialog.ShowDialog();

            if (folderBrowserDialog.SelectedPath != string.Empty)
            {
                return folderBrowserDialog.SelectedPath;
            }

            return null;
        }
    }
}
