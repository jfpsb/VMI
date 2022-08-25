using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using VandaModaIntimaWpf.Model.DAO;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel
{
    public class DespesaExcelStrategy : AExcelStrategy<Model.Despesa>
    {
        private ISession _session;
        private DAODespesa daoDespesa;
        private string[] _colunasEmpresarial = new string[] { "Data", "Data De Vencimento", "Representante", "Fornecedor", "Loja", "Descrição", "Valor" };
        private string[] _colunasFamiliar = new string[] { "Data", "Descrição", "Membro Familiar", "Valor" };
        private string[] _colunasResidencialOutras = new string[] { "Data", "Data De Vencimento", "Descrição", "Valor" };
        private string[] _colunasResumido = new string[] { "Descrição", "Valor" };
        private string[] _colunasFamiliarResumido = new string[] { "Membro Familiar", "Valor" };
        public DespesaExcelStrategy(ISession session)
        {
            _session = session;
            daoDespesa = new DAODespesa(session);
        }

        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override async void EscreveDados(Workbook workbook,
            CancellationToken token,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<Model.Despesa>[] containers)
        {
            descricao.Report("Iniciando exportação em Excel de Despesas");
            isIndeterminada.Report(true);

            WorksheetContainer<Model.Despesa> f = containers[0];
            double totalGeral = await daoDespesa.RetornaSomaTodasDespesas(f.Lista[0].Data);
            workbook.Worksheets.Add(Missing.Value, Missing.Value, 2, Missing.Value);

            isIndeterminada.Report(false);
            double incrementoProgresso = 0;

            for (int i = 0; i < containers.Length; i++)
            {
                token.ThrowIfCancellationRequested();

                int linha = 1;
                WorksheetContainer<Model.Despesa> container = containers[i];
                dynamic listaResumido;

                Worksheet worksheet = workbook.Worksheets.Item[i + 1];
                worksheet.Name = container.Nome;
                worksheet.Cells.Font.Name = "Gotham";

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

                switch (container.Nome)
                {
                    case "DESPESA EMPRESARIAL":
                        {
                            EscreveTextoRelatorio(worksheet, linha, 7, 9, 10);
                            linha++;
                            EscreveHeaders(worksheet, _colunasEmpresarial, linha, 1);
                            EscreveHeaders(worksheet, _colunasResumido, linha, 9);
                            listaResumido = container.Lista.GroupBy(g => g.Descricao).Select(s => new { Valor = s.Sum(sum => sum.Valor), GroupByKey = s.Key }).OrderBy(o => o.GroupByKey).ToList();
                            linha++;

                            valor.Report(-1); //Reseta valor ao passar valor negativo
                            incrementoProgresso = 100.0 / container.Lista.Count;
                            for (int j = 0; j < container.Lista.Count; j++)
                            {
                                token.ThrowIfCancellationRequested();
                                descricao.Report($"Escrevendo planilha {i + 1} de {containers.Length} - DESPESA EMPRESARIAL {j + 1} de {container.Lista.Count}");
                                worksheet.Cells[j + linha, 1] = container.Lista[j].Data;
                                worksheet.Cells[j + linha, 2] = container.Lista[j].DataVencimento;
                                worksheet.Cells[j + linha, 3] = container.Lista[j].Representante?.Nome;
                                worksheet.Cells[j + linha, 4] = container.Lista[j].Fornecedor?.Nome;
                                worksheet.Cells[j + linha, 5] = container.Lista[j].Loja?.Nome;
                                worksheet.Cells[j + linha, 6] = container.Lista[j].Descricao;
                                worksheet.Cells[j + linha, 7] = container.Lista[j].Valor;
                                valor.Report(incrementoProgresso);
                            }

                            valor.Report(-1); //Reseta valor ao passar valor negativo
                            incrementoProgresso = 100.0 / listaResumido.Count;
                            for (int j = 0; j < listaResumido.Count; j++)
                            {
                                token.ThrowIfCancellationRequested();
                                descricao.Report($"Escrevendo planilha {i + 1} de {containers.Length} - DESPESA EMPRESARIAL RESUMIDO {j + 1} de {listaResumido.Count}");
                                worksheet.Cells[j + linha, 9] = listaResumido[j].GroupByKey;
                                worksheet.Cells[j + linha, 10] = listaResumido[j].Valor;
                                valor.Report(incrementoProgresso);
                            }
                            EstilizaLinhas(worksheet, linha, container, listaResumido, 7, 9, 10);
                            break;
                        }
                    case "DESPESA FAMILIAR":
                        EscreveTextoRelatorio(worksheet, linha, 4, 6, 7);
                        linha++;
                        EscreveHeaders(worksheet, _colunasFamiliar, linha, 1);
                        EscreveHeaders(worksheet, _colunasFamiliarResumido, linha, 6);
                        listaResumido = container.Lista.GroupBy(g => g.Familiar.Nome).Select(s => new { Valor = s.Sum(sum => sum.Valor), GroupByKey = s.Key }).OrderBy(o => o.GroupByKey).ToList();
                        linha++;

                        valor.Report(-1); //Reseta valor ao passar valor negativo
                        incrementoProgresso = 100.0 / container.Lista.Count;
                        for (int j = 0; j < container.Lista.Count; j++)
                        {
                            token.ThrowIfCancellationRequested();
                            descricao.Report($"Escrevendo planilha {i + 1} de {containers.Length} - DESPESA FAMILIAR {j + 1} de {container.Lista.Count}");
                            worksheet.Cells[j + linha, 1] = container.Lista[j].Data;
                            worksheet.Cells[j + linha, 2] = container.Lista[j].Descricao;
                            worksheet.Cells[j + linha, 3] = container.Lista[j].Familiar.Nome;
                            worksheet.Cells[j + linha, 4] = container.Lista[j].Valor;
                            valor.Report(incrementoProgresso);
                        }

                        valor.Report(-1); //Reseta valor ao passar valor negativo
                        incrementoProgresso = 100.0 / listaResumido.Count;
                        for (int j = 0; j < listaResumido.Count; j++)
                        {
                            token.ThrowIfCancellationRequested();
                            descricao.Report($"Escrevendo planilha {i + 1} de {containers.Length} - DESPESA FAMILIAR RESUMIDO {j + 1} de {listaResumido.Count}");
                            worksheet.Cells[j + linha, 6] = listaResumido[j].GroupByKey;
                            worksheet.Cells[j + linha, 7] = listaResumido[j].Valor;
                            valor.Report(incrementoProgresso);
                        }
                        EstilizaLinhas(worksheet, linha, container, listaResumido, 4, 6, 7);
                        break;
                    default:
                        EscreveTextoRelatorio(worksheet, linha, 4, 6, 7);
                        linha++;
                        EscreveHeaders(worksheet, _colunasResidencialOutras, linha, 1);
                        EscreveHeaders(worksheet, _colunasResumido, linha, 6);
                        listaResumido = container.Lista.GroupBy(g => g.Descricao).Select(s => new { Valor = s.Sum(sum => sum.Valor), GroupByKey = s.Key }).OrderBy(o => o.GroupByKey).ToList();
                        linha++;

                        valor.Report(-1); //Reseta valor ao passar valor negativo
                        incrementoProgresso = 100.0 / container.Lista.Count;
                        for (int j = 0; j < container.Lista.Count; j++)
                        {
                            token.ThrowIfCancellationRequested();
                            descricao.Report($"Escrevendo planilha {i + 1} de {containers.Length} - DESPESA RESIDENCIAL {j + 1} de {container.Lista.Count}");
                            worksheet.Cells[j + linha, 1] = container.Lista[j].Data;
                            worksheet.Cells[j + linha, 2] = container.Lista[j].DataVencimento;
                            worksheet.Cells[j + linha, 3] = container.Lista[j].Descricao;
                            worksheet.Cells[j + linha, 4] = container.Lista[j].Valor;
                            valor.Report(incrementoProgresso);
                        }

                        valor.Report(-1); //Reseta valor ao passar valor negativo
                        incrementoProgresso = 100.0 / listaResumido.Count;
                        for (int j = 0; j < listaResumido.Count; j++)
                        {
                            token.ThrowIfCancellationRequested();
                            descricao.Report($"Escrevendo planilha {i + 1} de {containers.Length} - DESPESA RESIDENCIAL RESUMIDO {j + 1} de {listaResumido.Count}");
                            worksheet.Cells[j + linha, 6] = listaResumido[j].GroupByKey;
                            worksheet.Cells[j + linha, 7] = listaResumido[j].Valor;
                            valor.Report(incrementoProgresso);
                        }
                        EstilizaLinhas(worksheet, linha, container, listaResumido, 4, 6, 7);
                        break;
                }

                AutoFitColunas(worksheet);
            }
        }

        private void EstilizaLinhas(Worksheet worksheet, int linha, WorksheetContainer<Model.Despesa> container, dynamic listaResumido
            , int finalRange1, int inicioRange2, int finalRange2)
        {
            Range range3 = worksheet.Range[worksheet.Cells[linha, 1], worksheet.Cells[linha + container.Lista.Count - 1, finalRange1]];
            range3.Borders.Color = Color.Black;

            Range range8 = worksheet.Range[worksheet.Cells[linha, inicioRange2], worksheet.Cells[linha + listaResumido.Count - 1, finalRange2]];
            range8.Borders.Color = Color.Black;

            Range range4 = worksheet.Range[worksheet.Cells[linha, finalRange1], worksheet.Cells[linha + container.Lista.Count - 1, finalRange1]];
            Range range7 = worksheet.Range[worksheet.Cells[linha, finalRange2], worksheet.Cells[linha + listaResumido.Count - 1, finalRange2]];
            range7.NumberFormat = range4.NumberFormat = "R$ #.##0,00";
            range7.NumberFormatLocal = range4.NumberFormatLocal = "R$ #.##0,00";
        }

        private void EscreveTextoRelatorio(Worksheet worksheet, int linha, int finalRange1, int inicioRange2, int finalRange2)
        {
            worksheet.Cells[linha, 1] = "RELATÓRIO DETALHADO";
            worksheet.Cells[linha, 1].Font.Bold = true;
            worksheet.Cells[linha, 1].Font.Size = 15;
            Range range5 = worksheet.Range[worksheet.Cells[linha, 1], worksheet.Cells[linha, finalRange1]];
            range5.Merge();
            range5.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            range5.Borders.Color = Color.Black;
            range5.Interior.Color = Color.Yellow;

            worksheet.Cells[linha, inicioRange2] = "RELATÓRIO RESUMIDO";
            worksheet.Cells[linha, inicioRange2].Font.Bold = true;
            worksheet.Cells[linha, inicioRange2].Font.Size = 15;
            Range range6 = worksheet.Range[worksheet.Cells[linha, inicioRange2], worksheet.Cells[linha, finalRange2]];
            range6.Merge();
            range6.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            range6.Borders.Color = Color.Black;
            range6.Interior.Color = Color.Yellow;
        }
    }
}
