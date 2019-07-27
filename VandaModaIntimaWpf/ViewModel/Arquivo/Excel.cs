using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class Excel<IModel> where IModel : class
    {
        private IExportarExcelStrategy exportarExcelStrategy;
        private Application Aplicacao; // Aplicação Excel
        private Workbook Workbook; // Pasta
        private Worksheet Worksheet; // Planilha
        public Excel(IExportarExcelStrategy exportarExcelStrategy)
        {
            this.exportarExcelStrategy = exportarExcelStrategy;

            Aplicacao = new Application() { DisplayAlerts = false }; //DisplayAlerts em falso impede que apareça a mensagem perguntando se quero sobescrever o arquivo
            Workbook = Aplicacao.Workbooks.Add(Missing.Value);
            Worksheet = Workbook.Worksheets.Item[1];

            //Configurações visuais para célula
            Worksheet.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            Worksheet.Cells.Font.Name = "Century Gothic";
            Worksheet.Cells.Font.Size = 12;
            Worksheet.Cells.NumberFormat = "@";
        }

        public void Salvar(IList<IModel> lista)
        {
            exportarExcelStrategy.EscreveDados(Worksheet, lista);

            try
            {
                //Primeiro parâmetro é string vazia para abrir automaticamente a tela de salvar
                Workbook.SaveAs("", XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value,
                    false,
                    false,
                    XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, true, Missing.Value,
                    Missing.Value, Missing.Value);
            }
            catch (COMException ce)
            {
                Console.WriteLine(ce.Message);
            }
            finally
            {
                Workbook.Close(true, Missing.Value, Missing.Value);
                Aplicacao.Quit();
            }
        }
    }
}
