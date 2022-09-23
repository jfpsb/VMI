using System.Windows;
using System.Windows.Forms;
using VandaModaIntimaWpf.View.Interfaces;

namespace VandaModaIntimaWpf.View.PontoEletronico
{
    /// <summary>
    /// Interaction logic for TelaExportarImprimirPontoEletronico.xaml
    /// </summary>
    public partial class TelaExportarImprimirPontoEletronico : Window, IFolderBrowserDialog
    {
        public TelaExportarImprimirPontoEletronico()
        {
            InitializeComponent();
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
