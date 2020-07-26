using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using FolhaModel = VandaModaIntimaWpf.Model.FolhaPagamento;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class FolhaPagamentoExcelStrategy : IExcelStrategy
    {
        public void ConfiguraColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public void EscreveDados(Worksheet Worksheet, object l)
        {
            var lista = (IList<FolhaModel>)l;
            int linhaAtual = 1;
            int pColuna = 1; //Posição da primeira coluna da folha
            int sColuna = 2; //Posição da segunda coluna da folha

            Worksheet.Cells.Font.Size = 9;

            foreach (FolhaModel folha in lista)
            {
                Worksheet.Cells[linhaAtual, pColuna] = folha.Funcionario.Nome + " - " + folha.MesReferencia;
                Worksheet.Cells[linhaAtual, pColuna].Font.Bold = true;
                Worksheet.Cells[linhaAtual, pColuna].Font.Size = 10;
                Worksheet.Cells[linhaAtual, pColuna].Interior.Color = Color.LightGray;
                //Mescla células com o nome do funcionário, células já são centralizadas por padrão
                Worksheet.Range[Worksheet.Cells[linhaAtual, pColuna], Worksheet.Cells[linhaAtual, sColuna]].Merge();
                Worksheet.Range[Worksheet.Cells[linhaAtual, pColuna], Worksheet.Cells[linhaAtual, sColuna]].Borders.Color = Color.Black;

                linhaAtual++;

                Range pCelula = Worksheet.Cells[linhaAtual, pColuna]; //Primeira célula desta folha

                Worksheet.Cells[linhaAtual, pColuna] = "Salário";
                Worksheet.Cells[linhaAtual, sColuna] = folha.Funcionario.Salario;

                linhaAtual++;

                foreach (var parcela in folha.Parcelas)
                {
                    Adiantamento ad = parcela.Adiantamento;

                    Worksheet.Cells[linhaAtual, pColuna] = string.Format("Adiantamento em {0} de R$ {1} - Parcela {2} de {3}",
                        ad.Data.ToString("d"), ad.Valor, parcela.Numero, ad.Parcelas.Count);
                    Worksheet.Cells[linhaAtual, sColuna] = parcela.Valor;

                    Worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.Red;
                    Worksheet.Cells[linhaAtual, pColuna].Interior.Color = Color.Yellow;
                    Worksheet.Cells[linhaAtual, sColuna].Font.Color = Color.Red;
                    Worksheet.Cells[linhaAtual, sColuna].Interior.Color = Color.Yellow;
                    linhaAtual++;
                }

                Worksheet.Cells[linhaAtual, pColuna] = "Total Em Adiantamentos";
                Worksheet.Cells[linhaAtual, sColuna] = folha.TotalAdiantamentos;
                Worksheet.Cells[linhaAtual, pColuna].Interior.Color = Color.SkyBlue;
                Worksheet.Cells[linhaAtual, sColuna].Interior.Color = Color.SkyBlue;
                Worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.Red;
                Worksheet.Cells[linhaAtual, sColuna].Font.Color = Color.Red;

                linhaAtual++;

                Worksheet.Cells[linhaAtual, pColuna] = "Total A Receber";
                Worksheet.Cells[linhaAtual, sColuna] = folha.ValorAPagar;

                Worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.Blue;
                Worksheet.Cells[linhaAtual, sColuna].Font.Color = Color.Blue;

                Range uCelula = Worksheet.Cells[linhaAtual, sColuna]; //Última célula desta folha

                Worksheet.Range[pCelula, uCelula].Borders.LineStyle = XlLineStyle.xlContinuous;

                linhaAtual += 2;
            }
        }

        public string[] GetColunas()
        {
            return null;
        }

        public Task<bool> LeEInsereDados(Worksheet Worksheet)
        {
            throw new NotImplementedException();
        }
    }
}
