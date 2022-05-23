using Microsoft.Office.Interop.Excel;
using System;
using System.Threading;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel
{
    public class FuncionarioExcelStrategy : AExcelStrategy<Model.Funcionario>
    {
        private string[] _colunas = new[] { "Admissão", "CPF", "CTPS", "PIS/PASEP/NIT", "Nome", "Loja Contratado", "Loja Trabalho", "Telefone", "E-mail", "Endereço" };
        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override void EscreveDados(Workbook workbook,
            CancellationToken token,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<Model.Funcionario>[] containers)
        {
            descricao.Report("Iniciando exportação em Excel de Funcionário");
            isIndeterminada.Report(true);

            WorksheetContainer<Model.Funcionario> wscontainer = containers[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            EscreveHeaders(worksheet, _colunas, 1, 1);

            isIndeterminada.Report(false);
            double incrementoProgresso = 100.0 / lista.Count;

            for (int i = 0; i < lista.Count; i++)
            {
                descricao.Report($"Escrevendo funcionário {i + 1} de {lista.Count}");

                worksheet.Cells[i + 2, Model.Funcionario.Colunas.Admissao] = lista[i].Admissao.Value.Date.ToString("d");
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.Cpf] = $"'{lista[i].Cpf}";
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.Ctps] = lista[i].Ctps;
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.PIS] = $"'{lista[i].PisPasepNit}";
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.Nome] = lista[i].Nome;
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.LojaContratado] = lista[i].Loja.Nome;
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.LojaTrabalho] = lista[i].LojaTrabalho.Nome;
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.Telefone] = lista[i].Telefone;
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.Email] = lista[i].Email;
                worksheet.Cells[i + 2, Model.Funcionario.Colunas.Endereco] = lista[i].Endereco;

                valor.Report(incrementoProgresso);
            }

            AutoFitColunas(worksheet);
        }
    }
}
