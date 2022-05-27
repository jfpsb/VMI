using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using VandaModaIntimaWpf.View.Interfaces;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View
{
    public class APesquisarView2 : System.Windows.Controls.UserControl, IOpenFileDialog, ISaveFileDialog, IFolderBrowserDialog, ICloseable
    {
        protected Window window;

        public void Pesquisar_Closing(object sender, CancelEventArgs e)
        {
            //Fecha sessao
            if (DataContext is IPesquisarVM)
                (DataContext as IPesquisarVM).DisposeSession();
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

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }

            return null;
        }

        public void Close()
        {
            window.Close();
        }

        protected BitmapImage GetIcon(string path)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/VandaModaIntimaWpf;component{path}"));
        }
    }
}
