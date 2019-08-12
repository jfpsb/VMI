using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class ExportarFornecedorExcelStrategy : IExportarExcelStrategy
    {
        public void EscreveDados(Worksheet Worksheet, object l)
        {
            var lista = (IList<FornecedorModel>)l;

            for (int i = 0; i < lista.Count; i++)
            {
                Worksheet.Cells[i + 2, FornecedorModel.Colunas.CNPJ] = lista[i].Cnpj;
                Worksheet.Cells[i + 2, FornecedorModel.Colunas.Nome] = lista[i].Nome;
                Worksheet.Cells[i + 2, FornecedorModel.Colunas.NomeFantasia] = lista[i].NomeFantasia;
                Worksheet.Cells[i + 2, FornecedorModel.Colunas.Email] = lista[i].Email;
            }
        }

        public string[] GetColunas()
        {
            return new FornecedorModel().GetColunas();
        }

        public bool LeEInsereDados(Worksheet Worksheet)
        {
            throw new System.NotImplementedException();
        }
    }
}
