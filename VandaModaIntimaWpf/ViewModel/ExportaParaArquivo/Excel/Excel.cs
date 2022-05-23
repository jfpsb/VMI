using Microsoft.Office.Interop.Excel;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel
{
    public class Excel<T> where T : AModel, Model.IModel
    {
        private AExcelStrategy<T> exportaExcelStrategy;
        private Application Aplicacao; // Aplicação Excel
        private Workbook Workbook; // Pasta
        private string _nomePlanilha; //Deve incluir o caminho completo onde será salvo o arquivo no sistema, incluindo nome
        //private Worksheet Worksheet; // Planilha
        // Construtor para exportação
        public Excel(AExcelStrategy<T> exportaExcelStrategy, string caminhoPlanilha)
        {
            this.exportaExcelStrategy = exportaExcelStrategy;
            _nomePlanilha = caminhoPlanilha;
            Aplicacao = new Application() { DisplayAlerts = false }; //DisplayAlerts em falso impede que apareça a mensagem perguntando se quero sobescrever o arquivo
            Workbook = Aplicacao.Workbooks.Add(Missing.Value);
        }
        public Task Salvar(CancellationToken token,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<T>[] listas)
        {
            Task task = Task.Run(() =>
            {
                exportaExcelStrategy.EscreveDados(Workbook, token, descricao, valor, isIndeterminada, listas);

                try
                {
                    descricao.Report("Salvando arquivo Excel");
                    Workbook.SaveAs(_nomePlanilha, XlFileFormat.xlOpenXMLWorkbook, Missing.Value, Missing.Value,
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
            }, token);

            task.ContinueWith(t =>
            {
                if (token.IsCancellationRequested)
                {
                    try
                    {
                        Workbook.Close(false, Missing.Value, Missing.Value);
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
    }
}
