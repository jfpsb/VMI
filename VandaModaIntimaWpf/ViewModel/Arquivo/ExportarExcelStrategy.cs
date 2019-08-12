using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class ExportarExcelStrategy
    {
        private IExportarExcelStrategy exportarExcelStrategy;

        public ExportarExcelStrategy(IExportarExcelStrategy exportarExcelStrategy)
        {
            this.exportarExcelStrategy = exportarExcelStrategy;
        }

        public void EscreveDados(Worksheet Worksheet, object l)
        {
            string[] colunas = exportarExcelStrategy.GetColunas();

            //Escreve cabeçalho baseado nas colunas do model e estiliza
            for (int i = 0; i < colunas.Length; i++)
            {
                Worksheet.Cells[1, i + 1] = colunas[i];
                Worksheet.Cells[1, i + 1].Font.Bold = true;
                Worksheet.Cells[1, i + 1].Font.Size = 14;
                Worksheet.Cells[1, i + 1].Interior.Color = Color.LightGray;
                Worksheet.Cells[1, i + 1].Borders.Color = Color.Black;
            }

            exportarExcelStrategy.EscreveDados(Worksheet, l);

            Worksheet.Columns.AutoFit();
        }

        public Boolean LeDados(Worksheet Worksheet)
        {
            return exportarExcelStrategy.LeEInsereDados(Worksheet);
        }
    }
}
