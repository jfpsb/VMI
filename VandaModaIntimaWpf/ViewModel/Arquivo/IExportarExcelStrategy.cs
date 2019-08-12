using Microsoft.Office.Interop.Excel;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public interface IExportarExcelStrategy
    {
        string[] GetColunas();
        void EscreveDados(Worksheet Worksheet, object l);
        bool LeEInsereDados(Worksheet Worksheet);
    }
}
