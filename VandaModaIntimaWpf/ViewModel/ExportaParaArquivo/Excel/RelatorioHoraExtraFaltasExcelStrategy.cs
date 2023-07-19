using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel
{
    public class RelatorioHoraExtraFaltasExcelStrategy : AExcelStrategy<Model.FolhaPagamento>
    {
        private ISession _session;
        private DAOFaltas daoFaltas;
        private DAOHoraExtra daoHoraExtra;

        public RelatorioHoraExtraFaltasExcelStrategy(ISession session)
        {
            _session = session;
            daoFaltas = new DAOFaltas(session);
            daoHoraExtra = new DAOHoraExtra(session);
        }

        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public async override void EscreveDados(Workbook workbook, CancellationToken token, IProgress<string> descricao, IProgress<double> valor, IProgress<bool> isIndeterminada, params WorksheetContainer<Model.FolhaPagamento>[] listas)
        {
            descricao.Report("Iniciando exportação em Excel de dados de hora extra, faltas, etc");
            isIndeterminada.Report(true);
            WorksheetContainer<Model.FolhaPagamento> wscontainer = listas[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;
            int linhaAtual = 2;
            int pColuna = 1; //Posição da primeira coluna da folha
            int sColuna = 2; //Posição da segunda coluna da folha

            isIndeterminada.Report(false);
            double incrementoProgresso = 100.0 / lista.Count;

            Style estiloHeader = workbook.Styles.Add("EstiloHeader");
            estiloHeader.Font.Size = 14;
            estiloHeader.Interior.Color = Color.Yellow;
            estiloHeader.Borders.Color = Color.Black;
            estiloHeader.Font.Bold = true;

            Style estiloComum = workbook.Styles.Add("EstiloComum");
            estiloHeader.Font.Size = 12;
            estiloHeader.Borders.Color = Color.Black;
            estiloHeader.Font.Bold = true;

            int ordemRelatorios = 1; //Guarda a numeração de qual relatórió está sendo gerado no momento

            foreach (Model.FolhaPagamento folha in lista)
            {
                linhaAtual = 2;
                if (ordemRelatorios % 3 == 0) pColuna += 3; //Próximo relatório ficará duas colunas para a direita

                token.ThrowIfCancellationRequested();
                descricao.Report($"Gerando relatório de {folha.Funcionario.Nome}");

                worksheet.Cells[linhaAtual, pColuna] = $"EMPRESA: {folha.Funcionario.Loja.Nome}";
                worksheet.Range[worksheet.Cells[linhaAtual, pColuna], worksheet.Cells[linhaAtual, pColuna + 1]].Merge(Type.Missing);
                worksheet.Cells[linhaAtual, pColuna].Style = estiloHeader;
                linhaAtual++;

                worksheet.Cells[linhaAtual, pColuna] = $"FUNCIONÁRIO: {folha.Funcionario.Nome}";
                worksheet.Range[worksheet.Cells[linhaAtual, pColuna], worksheet.Cells[linhaAtual, pColuna + 1]].Merge(Type.Missing);
                worksheet.Cells[linhaAtual, pColuna].Style = estiloHeader;
                linhaAtual++;

                var falta = await daoFaltas.ListarFaltasPorMesFuncionarioSoma(folha.Ano, folha.Mes, folha.Funcionario);
                var possuiHorasExtras = (await daoHoraExtra.ListarPorMesFuncionario(folha.Ano, folha.Mes, folha.Funcionario)).Any();

                //Exclui do relatório funcionários que não possuem bônus que são pagos em folha, não possuem faltas, nem horas extras.
                if (!folha.Bonus.Any(a => a.PagoEmFolha) && falta == null && !possuiHorasExtras)
                {
                    continue;
                }

                worksheet.Cells[linhaAtual, pColuna] = "FALTAS";
                worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.Red;
                worksheet.Cells[linhaAtual, pColuna + 1].Font.Color = Color.Red;

                if (falta == null) //Não existem faltas
                {
                    worksheet.Cells[linhaAtual, pColuna + 1] = "NÃO POSSUI";
                }
                else
                {
                    worksheet.Cells[linhaAtual, pColuna + 1] = falta.TotalEmString;
                }

                linhaAtual++;

                IList<Model.HoraExtra> horasExtras = await daoHoraExtra.ListarPorMesFuncionarioGroupByTipoHoraExtra(folha.Ano, folha.Mes, folha.Funcionario);

                var horaextra50 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Contains("C/050")).FirstOrDefault();
                var horaextra60 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Contains("C/060")).FirstOrDefault();
                var horaextra100 = horasExtras.Where(w => w.TipoHoraExtra.Descricao.Contains("C/100")).FirstOrDefault();

                worksheet.Cells[linhaAtual, pColuna] = "HORA EXTRA 50%";
                worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.MediumBlue;
                worksheet.Cells[linhaAtual, pColuna + 1].Font.Color = Color.MediumBlue;

                if (horaextra50 == null)
                {
                    worksheet.Cells[linhaAtual, pColuna + 1] = "NÃO POSSUI";
                }
                else
                {
                    worksheet.Cells[linhaAtual, pColuna + 1] = horaextra50.TotalEmString;
                }

                linhaAtual++;

                worksheet.Cells[linhaAtual, pColuna] = "HORA EXTRA 60%";
                worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.MediumPurple;
                worksheet.Cells[linhaAtual, pColuna + 1].Font.Color = Color.MediumPurple;

                if (horaextra60 == null)
                {
                    worksheet.Cells[linhaAtual, pColuna + 1] = "NÃO POSSUI";
                }
                else
                {
                    worksheet.Cells[linhaAtual, pColuna + 1] = horaextra60.TotalEmString;
                }

                linhaAtual++;

                worksheet.Cells[linhaAtual, pColuna] = "HORA EXTRA 100%";
                worksheet.Cells[linhaAtual, pColuna].Font.Color = Color.SeaGreen;
                worksheet.Cells[linhaAtual, pColuna + 1].Font.Color = Color.SeaGreen;

                if (horaextra100 == null)
                {
                    worksheet.Cells[linhaAtual, pColuna + 1] = "NÃO POSSUI";
                }
                else
                {
                    worksheet.Cells[linhaAtual, pColuna + 1] = horaextra100.TotalEmString;
                }

                linhaAtual++;

                worksheet.Cells[linhaAtual, pColuna] = $"COMISSÕES E OUTROS BÔNUS";
                worksheet.Range[worksheet.Cells[linhaAtual, pColuna], worksheet.Cells[linhaAtual, pColuna + 1]].Merge(Type.Missing);
                worksheet.Cells[linhaAtual, pColuna].Style = estiloHeader;
                linhaAtual++;

                var Bonus = folha.Bonus;

                if (Bonus.Count != 0)
                {
                    worksheet.Cells[linhaAtual, pColuna] = $"NÃO HÁ BÔNUS";
                    worksheet.Range[worksheet.Cells[linhaAtual, pColuna], worksheet.Cells[linhaAtual, pColuna + 1]].Merge(Type.Missing);
                    linhaAtual++;
                }
                else
                {
                    if (folha.Funcionario.Funcao.Nome.Equals("VENDEDOR"))
                    {
                        Model.Bonus bonusAgregado = new Model.Bonus();
                        double soma = Bonus.Where(w => w.PagoEmFolha && (w.Descricao.StartsWith("COMISSÃO") || w.Descricao.StartsWith("ADICIONAL"))).Sum(s => s.Valor);

                        var b = Bonus.Where(w => w.Descricao.StartsWith("COMISSÃO") || w.Descricao.StartsWith("ADICIONAL")).FirstOrDefault();

                        foreach (var bonus in Bonus.Where(w => w.PagoEmFolha))
                        {
                            if (bonus.Descricao.StartsWith("COMISSÃO") || bonus.Descricao.StartsWith("ADICIONAL")
                                || bonus.Descricao.StartsWith("AUXÍLIO") || bonus.Descricao.StartsWith("PASSAGEM"))
                            {
                                continue;
                            }

                            worksheet.Cells[linhaAtual, pColuna] = bonus.Descricao.Replace(" (PAGO EM FOLHA)", "");
                            worksheet.Cells[linhaAtual, pColuna + 1] = bonus.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                            linhaAtual++;
                        }

                        if (soma != 0.0)
                        {
                            bonusAgregado.Data = b.Data;
                            bonusAgregado.Descricao = "COMISSÃO";
                            bonusAgregado.Valor = soma;

                            worksheet.Cells[linhaAtual, pColuna] = bonusAgregado.Descricao;
                            worksheet.Cells[linhaAtual, pColuna + 1] = bonusAgregado.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                            linhaAtual++;
                        }
                    }
                    else
                    {
                        foreach (var bonus in Bonus.Where(w => w.PagoEmFolha))
                        {
                            if (bonus.Descricao.StartsWith("AUXÍLIO") || bonus.Descricao.StartsWith("PASSAGEM"))
                            {
                                continue;
                            }

                            worksheet.Cells[linhaAtual, pColuna] = bonus.Descricao.Replace(" (PAGO EM FOLHA)", "");
                            worksheet.Cells[linhaAtual, pColuna + 1] = bonus.Valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                            linhaAtual++;
                        }
                    }
                }

                worksheet.Cells[linhaAtual, pColuna] = $"DESCONTOS";
                worksheet.Range[worksheet.Cells[linhaAtual, pColuna], worksheet.Cells[linhaAtual, pColuna + 1]].Merge(Type.Missing);
                worksheet.Cells[linhaAtual, pColuna].Style = estiloHeader;
                linhaAtual++;

                if (folha.Funcionario.RecebePassagem)
                {
                    worksheet.Cells[linhaAtual, pColuna] = $"DESCONTOS";
                    var ferias = folha.Funcionario.Ferias.Where(w => (w.Inicio.Month == folha.Mes && w.Inicio.Year == folha.Ano) ||
                    (w.Fim.Month == folha.Mes && w.Fim.Year == folha.Ano)).FirstOrDefault();
                    worksheet.Cells[linhaAtual, pColuna + 1] = $"DESCONTO DE PASSAGEM";

                    if (ferias != null)
                    {
                        worksheet.Cells[linhaAtual, pColuna + 1].Style.WrapText = true;
                        worksheet.Cells[linhaAtual, pColuna + 1] += $"(OBS: FÉRIAS DE {ferias.Inicio:dd/MM} A {ferias.Fim:dd/MM})";
                    }

                    linhaAtual++;

                    worksheet.Range[worksheet.Cells[linhaAtual, pColuna], worksheet.Cells[linhaAtual, pColuna + 1]].Merge(Type.Missing);
                }
            }
        }
    }
}
