using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel
{
    class EntradaMercadoriaPorFornecedorExcelStrategy : AExcelStrategy<EntradaMercadoriaProdutoGrade>
    {
        private string[] _colunas = new[] { "Cód. De Barras", "Cód. De Barras Alt.", "Descrição", "Grade", "Preço", "Quantidade" };
        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override void EscreveDados(Workbook workbook,
            CancellationToken token,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<EntradaMercadoriaProdutoGrade>[] containers)
        {
            descricao.Report("Iniciando exportação em Excel de Entradas De Mercadoria");
            isIndeterminada.Report(true);

            WorksheetContainer<Model.EntradaMercadoriaProdutoGrade> wscontainer = containers[0];
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

            isIndeterminada.Report(false);
            double incrementoProgresso = 100.0 / lista.Count;
            for (int i = 0; i < lista.Count; i++)
            {
                token.ThrowIfCancellationRequested();
                descricao.Report($"Escrevendo entrada de mercadoria {i + 1} de {lista.Count}");
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

                valor.Report(incrementoProgresso);
            }

            AutoFitColunas(worksheet);
        }
    }
}
