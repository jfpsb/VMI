using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public abstract class AExcelStrategy
    {
        public abstract void EscreveDados(Workbook workbook, params object[] listas);
        public abstract Task LeEInsereDados(Workbook workbook);
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
    }
}
