using Microsoft.Office.Interop.Excel;
using System;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    //TODO: Implementar Excel de Loja
    class LojaExcelStrategy : IExcelStrategy
    {
        public void ConfiguraColunas(Worksheet Worksheet)
        {
            throw new NotImplementedException();
        }

        public void EscreveDados(Worksheet Worksheet, object l)
        {
            throw new NotImplementedException();
        }

        public string[] GetColunas()
        {
            throw new NotImplementedException();
        }

        public Task<bool> LeEInsereDados(Worksheet Worksheet)
        {
            throw new NotImplementedException();
        }
    }
}
