using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Drawing;
using ProdutoModel = VandaModaIntimaWpf.Model.Produto;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class ProdutoExcelStrategy : AExcelStrategy<ProdutoModel>
    {
        private ISession _session;
        private string[] _colunasProduto = new[] { "Código", "Descrição", "Fornecedor", "Marca", "NCM" };
        private string[] _colunasProdutoGrade = new[] { "Cód. De Barras", "Cód. De Barras Alt.", "Preço Custo", "Preço Venda", "Descrição Grade" };

        public ProdutoExcelStrategy(ISession session)
        {
            _session = session;
        }
        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Range["A1", "E1"].EntireColumn.AutoFit();
        }
        public override void EscreveDados(Workbook workbook,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<ProdutoModel>[] containers)
        {
            descricao.Report("Iniciando exportação em Excel de Produtos");
            isIndeterminada.Report(true);
            WorksheetContainer<ProdutoModel> wscontainer = containers[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            double incrementoProgresso = 100.0 / lista.Count;
            int linhaOffset = 1;

            Style estiloCabecalho = workbook.Styles.Add("Cabeçalho Produto");
            estiloCabecalho.Font.Size = 14;
            estiloCabecalho.Interior.Color = Color.Yellow;
            estiloCabecalho.Borders.Color = Color.Black;
            estiloCabecalho.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = XlLineStyle.xlLineStyleNone;
            estiloCabecalho.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = XlLineStyle.xlLineStyleNone;
            estiloCabecalho.Font.Bold = true;

            Style estiloCabecalho2 = workbook.Styles.Add("Cabeçalho Grade");
            estiloCabecalho2.Font.Size = 14;
            estiloCabecalho2.Interior.Color = Color.LightGray;
            estiloCabecalho2.Borders.Color = Color.Black;
            estiloCabecalho2.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = XlLineStyle.xlLineStyleNone;
            estiloCabecalho2.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = XlLineStyle.xlLineStyleNone;
            estiloCabecalho2.Font.Bold = true;

            Style estiloDados = workbook.Styles.Add("Dados Grade");
            estiloDados.Font.Size = 12;
            estiloDados.Borders.Color = Color.Black;
            estiloDados.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = XlLineStyle.xlLineStyleNone;
            estiloDados.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = XlLineStyle.xlLineStyleNone;

            Style estiloDinheiro = workbook.Styles["Currency"];
            estiloDinheiro.Font.Size = 12;
            estiloDinheiro.Borders.Color = Color.Black;
            estiloDinheiro.Borders[XlBordersIndex.xlDiagonalDown].LineStyle = XlLineStyle.xlLineStyleNone;
            estiloDinheiro.Borders[XlBordersIndex.xlDiagonalUp].LineStyle = XlLineStyle.xlLineStyleNone;

            isIndeterminada.Report(false);
            valor.Report(-1); //Reseta valor ao passar valor negativo
            for (int i = 0; i < lista.Count; i++)
            {
                if (i != 0)
                    linhaOffset++;

                descricao.Report($"Escrevendo produto {i + 1} de {lista.Count}");

                worksheet.Cells[i + linhaOffset, 1] = "PRODUTO";
                worksheet.Cells[i + linhaOffset, 1].Style = estiloCabecalho;
                linhaOffset++;

                EscreveHeaders(worksheet, _colunasProduto, i + linhaOffset, 1, estiloCabecalho);
                linhaOffset++;

                Range produtoRange = worksheet.Range[worksheet.Cells[i + linhaOffset, 1], worksheet.Cells[i + linhaOffset, 5]];
                produtoRange.Style = estiloDados;

                worksheet.Cells[i + linhaOffset, ProdutoModel.Colunas.CodBarra] = lista[i].CodBarra;
                worksheet.Cells[i + linhaOffset, ProdutoModel.Colunas.Descricao] = lista[i].Descricao;
                worksheet.Cells[i + linhaOffset, ProdutoModel.Colunas.Fornecedor] = "NÃO HÁ FORNECEDOR";
                worksheet.Cells[i + linhaOffset, ProdutoModel.Colunas.Marca] = "NÃO HÁ MARCA";
                worksheet.Cells[i + linhaOffset, ProdutoModel.Colunas.Ncm] = "NÃO HÁ NCM";

                if (lista[i].Fornecedor != null)
                    worksheet.Cells[i + linhaOffset, ProdutoModel.Colunas.Fornecedor] = lista[i].Fornecedor.Nome;

                if (lista[i].Marca != null)
                    worksheet.Cells[i + linhaOffset, ProdutoModel.Colunas.Marca] = lista[i].Marca.Nome;

                if (!string.IsNullOrEmpty(lista[i].Ncm))
                    worksheet.Cells[i + linhaOffset, ProdutoModel.Colunas.Ncm] = lista[i].Ncm;

                linhaOffset++;

                worksheet.Cells[i + linhaOffset, 1] = "GRADES";
                worksheet.Cells[i + linhaOffset, 1].Style = estiloCabecalho2;
                linhaOffset++;

                EscreveHeaders(worksheet, _colunasProdutoGrade, i + linhaOffset, 1, estiloCabecalho2);

                linhaOffset++;

                Range gradesRange = worksheet.Range[worksheet.Cells[i + linhaOffset, 1], worksheet.Cells[i + linhaOffset + lista[i].Grades.Count - 1, 5]];
                gradesRange.Style = estiloDados;

                foreach (var grade in lista[i].Grades)
                {
                    worksheet.Cells[i + linhaOffset, 1] = grade.CodBarra;
                    worksheet.Cells[i + linhaOffset, 2] = grade.CodBarraAlternativo;
                    worksheet.Cells[i + linhaOffset, 3] = grade.PrecoCusto;
                    worksheet.Cells[i + linhaOffset, 3].Style = estiloDinheiro;
                    worksheet.Cells[i + linhaOffset, 4] = grade.Preco;
                    worksheet.Cells[i + linhaOffset, 4].Style = estiloDinheiro;
                    worksheet.Cells[i + linhaOffset, 5] = grade.SubGradesToShortString;
                    linhaOffset++;
                }

                valor.Report(incrementoProgresso);
            }

            AutoFitColunas(worksheet);
        }
    }
}
