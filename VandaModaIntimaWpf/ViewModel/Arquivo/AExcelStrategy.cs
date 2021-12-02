using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public abstract class AExcelStrategy
    {
        public abstract void EscreveDados(Workbook workbook, params object[] listas);
        public abstract Task<bool> LeEInsereDados(Workbook workbook);
        /// <summary>
        /// Configura o tamanho das colunas.
        /// Geralmente serão todas AutoFit, mas em alguns casos a coluna precisa ter um tamanho diferente ou propriedades diferentes.
        /// </summary>
        /// <param name="Worksheet"></param>
        public abstract void AutoFitColunas(Worksheet Worksheet);
        protected void EscreveColunas(Worksheet worksheet, string[] colunas, int linha)
        {
            //Escreve cabeçalho baseado nas colunas do model e estiliza
            for (int i = 0; i < colunas.Length; i++)
            {
                worksheet.Cells[linha, i + 1] = colunas[i];
                worksheet.Cells[linha, i + 1].Font.Bold = true;
                worksheet.Cells[linha, i + 1].Font.Size = 14;
                worksheet.Cells[linha, i + 1].Interior.Color = Color.LightGray;
                worksheet.Cells[linha, i + 1].Borders.Color = Color.Black;
            }
        }
    }
}
