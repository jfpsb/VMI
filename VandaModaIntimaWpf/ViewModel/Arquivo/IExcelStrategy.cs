using Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public interface IExcelStrategy
    {
        string[] GetColunas();
        void EscreveDados(Worksheet Worksheet, object l);
        Task<bool> LeEInsereDados(Worksheet Worksheet);
        /// <summary>
        /// Configura o tamanho das colunas.
        /// Geralmente serão todas AutoFit, mas em alguns casos a coluna precisa ter um tamanho diferente ou propriedades diferentes.
        /// </summary>
        /// <param name="Worksheet"></param>
        void ConfiguraColunas(Worksheet Worksheet);
    }
}
