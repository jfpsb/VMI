using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using MarcaModel = VandaModaIntimaWpf.Model.Marca.Marca;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class ExportarMarcaExcelStrategy : IExportarExcelStrategy
    {
        public void EscreveDados(Worksheet Worksheet, object l)
        {
            var lista = (IList<MarcaModel>)l;

            for(int i = 0; i < lista.Count; i++)
            {
                Worksheet.Cells[i + 2, MarcaModel.Colunas.Nome] = lista[i].Nome;
            }
        }

        public string[] GetColunas()
        {
            return new MarcaModel().GetColunas();
        }

        public bool LeEInsereDados(Worksheet Worksheet)
        {
            throw new System.NotImplementedException();
        }
    }
}
