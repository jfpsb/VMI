using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class Excel<IModel> where IModel : class
    {
        private ExcelStrategy exportaExcelStrategy;
        private Application Aplicacao; // Aplicação Excel
        private Workbook Workbook; // Pasta
        private Worksheet Worksheet; // Planilha
        public Excel(ExcelStrategy exportaExcelStrategy)
        {
            this.exportaExcelStrategy = exportaExcelStrategy;

            Aplicacao = new Application() { DisplayAlerts = false }; //DisplayAlerts em falso impede que apareça a mensagem perguntando se quero sobescrever o arquivo
            Workbook = Aplicacao.Workbooks.Add(Missing.Value);
            Worksheet = Workbook.Worksheets.Item[1];

            //Configurações visuais para célula (exceto cabeçalho, que fica em ExportaExcelStrategy)
            Worksheet.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            Worksheet.Cells.Font.Name = "Century Gothic";
            Worksheet.Cells.Font.Size = 12;
            Worksheet.Cells.NumberFormat = "@";
        }

        public Excel(ExcelStrategy exportaExcelStrategy, String path)
        {
            this.exportaExcelStrategy = exportaExcelStrategy;

            Aplicacao = new Application() { DisplayAlerts = false }; //DisplayAlerts em falso impede que apareça a mensagem perguntando se quero sobescrever o arquivo
            Workbook = Aplicacao.Workbooks.Open(path);
            Worksheet = Workbook.Worksheets.Item[1];
        }

        public void Salvar(IList<IModel> lista)
        {
            exportaExcelStrategy.EscreveDados(Worksheet, lista);

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

        public bool Importar()
        {
            bool result = false;
            try
            {
                result = exportaExcelStrategy.LeEInsereDados(Worksheet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Workbook.Close(true, Missing.Value, Missing.Value);
                Aplicacao.Quit();
            }

            return result;
        }
    }
}
