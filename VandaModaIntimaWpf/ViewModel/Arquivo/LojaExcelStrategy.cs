using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class LojaExcelStrategy : AExcelStrategy<Model.Loja>
    {
        private ISession _session;
        private string[] _colunas = new[] { "CNPJ", "Matriz", "Nome", "Telefone", "Endereço", "Inscrição Estadual" };

        public LojaExcelStrategy(ISession session)
        {
            _session = session;
        }

        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override void EscreveDados(Workbook workbook,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<Model.Loja>[] containers)
        {
            descricao.Report("Iniciando exportação em Excel de Loja");
            isIndeterminada.Report(true);

            WorksheetContainer<Model.Loja> wscontainer = containers[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            EscreveHeaders(worksheet, _colunas, 1, 1);

            isIndeterminada.Report(false);
            double incrementoProgresso = 100.0 / lista.Count;
            for (int i = 0; i < lista.Count; i++)
            {
                descricao.Report($"Escrevendo loja {i + 1} de {lista.Count}");
                worksheet.Cells[i + 2, Model.Loja.Colunas.Cnpj] = $"'{lista[i].Cnpj}";
                worksheet.Cells[i + 2, Model.Loja.Colunas.Matriz] = lista[i].Matriz?.Nome;
                worksheet.Cells[i + 2, Model.Loja.Colunas.Nome] = lista[i].Nome;
                worksheet.Cells[i + 2, Model.Loja.Colunas.Telefone] = lista[i].Telefone;
                worksheet.Cells[i + 2, Model.Loja.Colunas.Endereco] = lista[i].Endereco;
                worksheet.Cells[i + 2, Model.Loja.Colunas.InscricaoEstadual] = lista[i].InscricaoEstadual;
                valor.Report(incrementoProgresso);
            }

            AutoFitColunas(worksheet);
        }
    }
}
