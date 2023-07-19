using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;
using System.Threading;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel
{
    public abstract class AExcelStrategy<E> where E : AModel
    {
        public abstract void EscreveDados(Workbook workbook,
            CancellationToken token,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<E>[] listas);

        /// <summary>
        /// Configura o tamanho das colunas.
        /// Geralmente serão todas AutoFit, mas em alguns casos a coluna precisa ter um tamanho diferente ou propriedades diferentes.
        /// </summary>
        /// <param name="Worksheet"></param>
        public abstract void AutoFitColunas(Worksheet Worksheet);
        protected void EscreveHeaders(Worksheet worksheet, string[] colunas, int linha, int coluna)
        {
            //Escreve cabeçalho baseado nas colunas do model e estiliza
            for (int i = 0; i < colunas.Length; i++)
            {
                worksheet.Cells[linha, i + coluna] = colunas[i];
                worksheet.Cells[linha, i + coluna].Font.Bold = true;
                worksheet.Cells[linha, i + coluna].Font.Size = 14;
                worksheet.Cells[linha, i + coluna].Interior.Color = Color.LightGray;
                worksheet.Cells[linha, i + coluna].Borders.Color = Color.Black;
            }
        }

        protected void EscreveHeaders(Worksheet worksheet, string[] colunas, int linha, int coluna, Style estilo)
        {
            //Escreve cabeçalho baseado nas colunas do model e estiliza
            for (int i = 0; i < colunas.Length; i++)
            {
                worksheet.Cells[linha, i + coluna] = colunas[i];
                worksheet.Cells[linha, i + coluna].Style = estilo;
            }
        }
    }
}
