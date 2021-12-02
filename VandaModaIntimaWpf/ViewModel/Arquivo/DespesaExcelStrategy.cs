using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    public class DespesaExcelStrategy : AExcelStrategy
    {
        private ISession _session;
        private DAODespesa daoDespesa;
        private string[] _colunasEmpresarial = new string[] { "Data", "Data De Vencimento", "Representante", "Fornecedor", "Loja", "Descrição", "Valor" };
        private string[] _colunasEmpresarialResumido = new string[] { "Descrição", "Valor" };
        public DespesaExcelStrategy(ISession session)
        {
            _session = session;
            daoDespesa = new DAODespesa(session);
        }

        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override async void EscreveDados(Workbook workbook, params object[] containers)
        {
            WorksheetContainer<Model.Despesa> f = (WorksheetContainer<Model.Despesa>)containers[0];
            double totalGeral = await daoDespesa.RetornaSomaTodasDespesas(f.Lista[0].Data);
            workbook.Worksheets.Add(Missing.Value, Missing.Value, 3, Missing.Value);

            for (int i = 0; i < containers.Length; i++)
            {
                int linha = 1;
                WorksheetContainer<Model.Despesa> container = (WorksheetContainer<Model.Despesa>)containers[i];
                var listaResumido = container.Lista.GroupBy(g => g.Descricao).Select(s => new { Valor = s.Sum(sum => sum.Valor), Descricao = s.Key }).OrderBy(o => o.Descricao).ToList();

                Worksheet worksheet = workbook.Worksheets.Item[i + 1];
                worksheet.Name = container.Nome;
                worksheet.Cells.Font.Name = "Century Gothic";

                Range range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[2, 2]];
                range.Borders.Color = Color.Black;
                range.Interior.Color = Color.Yellow;

                Range range2 = worksheet.Range[worksheet.Cells[1, 2], worksheet.Cells[2, 2]];
                range2.NumberFormat = "R$ #.###,00";
                range2.NumberFormatLocal = "R$ #.###,00";

                worksheet.Cells[linha, 1] = "TOTAL GERAL EM DESPESAS";
                worksheet.Cells[linha, 1].Font.Size = 12;
                worksheet.Cells[linha, 1].Font.Bold = true;
                worksheet.Cells[linha, 2] = totalGeral;
                linha++;

                worksheet.Cells[linha, 1] = "TOTAL EM DESPESAS NA PLANILHA ATUAL";
                worksheet.Cells[linha, 1].Font.Bold = true;
                worksheet.Cells[linha, 2] = container.Lista.Sum(s => s.Valor);
                worksheet.Cells[linha, 1].Font.Size = 12;
                linha += 2;

                EscreveColunas(worksheet, _colunasEmpresarial, linha);
                linha++;

                Range range3 = worksheet.Range[worksheet.Cells[linha, 1], worksheet.Cells[linha + container.Lista.Count - 1, 7]];
                range3.Borders.Color = Color.Black;

                Range range4 = worksheet.Range[worksheet.Cells[linha, 7], worksheet.Cells[linha + container.Lista.Count - 1, 7]];
                range4.NumberFormat = "R$ #.###,00";
                range4.NumberFormatLocal = "R$ #.###,00";

                for (int j = 0; j < container.Lista.Count; j++)
                {
                    worksheet.Cells[j + linha, 1] = container.Lista[j].Data;
                    worksheet.Cells[j + linha, 2] = container.Lista[j].DataVencimento;
                    worksheet.Cells[j + linha, 3] = container.Lista[j].Representante?.Nome;
                    worksheet.Cells[j + linha, 4] = container.Lista[j].Fornecedor?.Nome;
                    worksheet.Cells[j + linha, 5] = container.Lista[j].Loja?.Nome;
                    worksheet.Cells[j + linha, 6] = container.Lista[j].Descricao;
                    worksheet.Cells[j + linha, 7] = container.Lista[j].Valor;
                }

                for (int j = 0; j < listaResumido.Count; j++)
                {
                    worksheet.Cells[j + linha, 9] = listaResumido[j].Descricao;
                    worksheet.Cells[j + linha, 10] = listaResumido[j].Valor;
                }

                AutoFitColunas(worksheet);
            }
        }

        public override Task<bool> LeEInsereDados(Workbook workbook)
        {
            throw new NotImplementedException();
        }
    }
}
