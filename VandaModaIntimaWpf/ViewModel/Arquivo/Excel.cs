using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class Excel<T> where T : class, Model.IModel
    {
        private AExcelStrategy exportaExcelStrategy;
        private Application Aplicacao; // Aplicação Excel
        private Workbook Workbook; // Pasta
        //private Worksheet Worksheet; // Planilha
        // Construtor para exportação
        public Excel(AExcelStrategy exportaExcelStrategy)
        {
            this.exportaExcelStrategy = exportaExcelStrategy;

            Aplicacao = new Application() { DisplayAlerts = false }; //DisplayAlerts em falso impede que apareça a mensagem perguntando se quero sobescrever o arquivo
            Workbook = Aplicacao.Workbooks.Add(Missing.Value);
            //Worksheet = Workbook.Worksheets.Item[1];

            ////Configurações visuais para célula (exceto cabeçalho, que fica em ExportaExcelStrategy)
            //Worksheet.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            //Worksheet.Cells.VerticalAlignment = XlHAlign.xlHAlignCenter;
            //Worksheet.Cells.Font.Name = "Century Gothic";
            //Worksheet.Cells.Font.Size = 12;
            //Worksheet.Cells.NumberFormat = "@";
        }
        // Construtor para importação
        public Excel(AExcelStrategy exportaExcelStrategy, string path)
        {
            this.exportaExcelStrategy = exportaExcelStrategy;

            Aplicacao = new Application() { DisplayAlerts = false }; //DisplayAlerts em falso impede que apareça a mensagem perguntando se quero sobrescrever o arquivo
            Workbook = Aplicacao.Workbooks.Open(path);
            //Worksheet = Workbook.Worksheets.Item[1];
        }

        public Task Salvar(params WorksheetContainer<T>[] listas)
        {
            Task task = Task.Run(() =>
            {
                exportaExcelStrategy.EscreveDados(Workbook, listas);

                try
                {
                    // Primeiro parâmetro é string vazia para abrir automaticamente a tela de salvar do Excel.
                    // Isso vai causar uma exceção, por isso os métodos Close e Quit estão no finally.
                    // A planilha só vai salvar de fato quando chamar Close
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
                    try
                    {
                        Workbook.Close(true, Missing.Value, Missing.Value);
                        Aplicacao.Quit();
                        Marshal.ReleaseComObject(Aplicacao);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            });

            return task;
        }

        public async Task Importar()
        {
            try
            {
                await exportaExcelStrategy.LeEInsereDados(Workbook);
            }
            catch (Exception e)
            {
                throw new Exception("", e);
            }
            finally
            {
                Workbook.Close(true, Missing.Value, Missing.Value);
                Aplicacao.Quit();
            }
        }
    }
}
