using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class ExcelStrategy
    {
        private IExcelStrategy excelStrategy;

        public ExcelStrategy(IExcelStrategy excelStrategy)
        {
            this.excelStrategy = excelStrategy;
        }

        public void EscreveDados(Worksheet Worksheet, object l)
        {
            string[] colunas = excelStrategy.GetColunas();

            if (colunas != null)
            {
                //Escreve cabeçalho baseado nas colunas do model e estiliza
                for (int i = 0; i < colunas.Length; i++)
                {
                    Worksheet.Cells[1, i + 1] = colunas[i];
                    Worksheet.Cells[1, i + 1].Font.Bold = true;
                    Worksheet.Cells[1, i + 1].Font.Size = 14;
                    Worksheet.Cells[1, i + 1].Interior.Color = Color.LightGray;
                    Worksheet.Cells[1, i + 1].Borders.Color = Color.Black;
                }
            }

            excelStrategy.EscreveDados(Worksheet, l);
            excelStrategy.ConfiguraColunas(Worksheet);
        }

        public async Task<bool> LeEInsereDados(Worksheet Worksheet)
        {
            return await excelStrategy.LeEInsereDados(Worksheet);
        }
    }
}
