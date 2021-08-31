using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.View
{
    public partial class APesquisarView : Window, IOpenFileDialog, ICloseable
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
                Dispatcher.InvokeShutdown();
            }
        }

        public string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return null;
        }
    }
}
