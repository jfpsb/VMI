using System;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using VandaModaIntimaWpf.View.Interfaces;

namespace VandaModaIntimaWpf.View
{
    public class ACadastrarView : System.Windows.Controls.UserControl, IOpenFileDialog
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

        protected BitmapImage GetIcon(string path)
        {
            return new BitmapImage(new Uri($"pack://application:,,,/VandaModaIntimaWpf;component{path}"));
        }

        protected void SetWidth(double width)
        {
            System.Windows.Application.Current.MainWindow.Width = width;
        }

        protected void SetHeight(double heigth)
        {
            System.Windows.Application.Current.MainWindow.Height = heigth;
        }
    }
}
