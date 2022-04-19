using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class EntradaMercadoriaPorFornecedorExcelStrategy : AExcelStrategy
    {
        private string[] _colunas = new[] { "Cód. De Barras", "Cód. De Barras Alt.", "Descrição", "Grade", "Preço", "Quantidade" };
        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override void EscreveDados(Workbook workbook, params object[] containers)
        {
            WorksheetContainer<Model.EntradaMercadoriaProdutoGrade> wscontainer = (WorksheetContainer<Model.EntradaMercadoriaProdutoGrade>)containers[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;
            worksheet.Cells.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            int linha = 1;

            worksheet.Cells[linha, 1].Font.Bold = true;
            worksheet.Cells[linha, 1].Font.Size = 14;
            worksheet.Cells[linha, 1] = "Fornecedor";

            worksheet.Cells[linha, 2].Font.Bold = true;
            worksheet.Cells[linha, 2].Font.Size = 14;
            worksheet.Cells[linha, 2] = lista[0].ProdutoGrade.Produto.Fornecedor.Nome;

            linha++;

            EscreveHeaders(worksheet, _colunas, linha++, 1);

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].ProdutoGrade == null)
                {
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Descricao] = lista[i].ProdutoDescricao;
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Grade] = lista[i].GradeDescricao;
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Preco].NumberFormat = "[$R$-pt-BR] #.##0,00";
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Preco] = lista[i].GradePreco;
                }
                else
                {
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.CodBarra] = $"'{lista[i].ProdutoGrade.CodBarra}";
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.CodBarraAlt] = lista[i].ProdutoGrade.CodBarraAlternativo;
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Descricao] = lista[i].ProdutoGrade.Produto.Descricao;
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Grade] = lista[i].ProdutoGrade.SubGradesToShortString;
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Preco].NumberFormat = "[$R$-pt-BR] #.##0,00";
                    worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Preco] = lista[i].ProdutoGrade.Preco;
                }
                worksheet.Cells[i + linha, EntradaMercadoriaProdutoGrade.Colunas.Quantidade] = lista[i].Quantidade;
            }

            AutoFitColunas(worksheet);
        }

        public override Task LeEInsereDados(Workbook workbook)
        {
            throw new NotImplementedException();
        }
    }
}
