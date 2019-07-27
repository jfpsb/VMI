using Microsoft.Office.Interop.Excel;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    interface IExportarExcelStrategy
    {
        void EscreveDados(Worksheet Worksheet, object l);
    }
}
