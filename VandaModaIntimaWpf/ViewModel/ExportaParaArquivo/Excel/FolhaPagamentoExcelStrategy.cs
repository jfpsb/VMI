using Microsoft.Office.Interop.Excel;
using System;
using System.Drawing;
using System.Threading;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel
{
    public class FolhaPagamentoExcelStrategy : AExcelStrategy<Model.FolhaPagamento>
    {
        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override void EscreveDados(Workbook workbook,
            CancellationToken token,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<Model.FolhaPagamento>[] containers)
        {
            descricao.Report("Iniciando exportação em Excel de Folhas De Pagamento");
            isIndeterminada.Report(true);
            WorksheetContainer<Model.FolhaPagamento> wscontainer = containers[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;
            int linhaAtual = 1;
            int pColuna = 1; //Posição da primeira coluna da folha
            int sColuna = 2; //Posição da segunda coluna da folha

            worksheet.Cells.Font.Size = 9;

            isIndeterminada.Report(false);
            double incrementoProgresso = 100.0 / lista.Count;
            int i = 1;
            foreach (Model.FolhaPagamento folha in lista)
            {
                token.ThrowIfCancellationRequested();
                descricao.Report($"Escrevendo folha de pagamento {i} de {lista.Count}");
                worksheet.Cells[linhaAtual, pColuna] = folha.Funcionario.Nome + " - " + folha.MesReferencia;
                worksheet.Cells[linhaAtual, pColuna].Font.Bold = true;
                worksheet.Cells[linhaAtual, pColuna].Font.Size = 10;
                worksheet.Cells[linhaAtual, pColuna].Interior.Color = Color.LightGray;
                //Mescla células com o nome do funcionário, células já são centralizadas por padrão
                worksheet.Range[worksheet.Cells[linhaAtual, pColuna], worksheet.Cells[linhaAtual, sColuna]].Merge();
                worksheet.Range[worksheet.Cells[linhaAtual, pColuna], worksheet.Cells[linhaAtual, sColuna]].Borders.Color = Color.Black;

                linhaAtual++;

                Range pCelula = worksheet.Cells[linhaAtual, pColuna]; //Primeira célula desta folha

                worksheet.Cells[linhaAtual, pColuna].Font.Bold = true;
                worksheet.Cells[linhaAtual, sColuna].Font.Bold = true;

                linhaAtual++;

                //TODO: arrumar exportacao de folha de pagamento

                worksheet.Cells[linhaAtual, pColuna] = "Total Em Adiantamentos";
                //Worksheet.Cells[linhaAtual, sColuna] = folha.TotalAdiantamentos;
                worksheet.Cells[linhaAtual, pColuna].Interior.Color = Color.SkyBlue;
                worksheet.Cells[linhaAtual, sColuna].Interior.Color = Color.SkyBlue;
                worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.Red;
                worksheet.Cells[linhaAtual, sColuna].Font.Color = Color.Red;

                linhaAtual++;

                worksheet.Cells[linhaAtual, pColuna] = "Total A Receber";
                //Worksheet.Cells[linhaAtual, sColuna] = folha.ValorAPagar;

                worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.Blue;
                worksheet.Cells[linhaAtual, sColuna].Font.Color = Color.Blue;

                Range uCelula = worksheet.Cells[linhaAtual, sColuna]; //Última célula desta folha

                worksheet.Range[pCelula, uCelula].Borders.LineStyle = XlLineStyle.xlContinuous;

                linhaAtual += 2;
                i++;
                valor.Report(incrementoProgresso);
            }
        }
    }
}
