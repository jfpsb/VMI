using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;

namespace VandaModaIntimaWpf.ViewModel.Arquivo
{
    class FornecedorExcelStrategy : AExcelStrategy<Model.Fornecedor>
    {
        private ISession _session;

        public FornecedorExcelStrategy(ISession session)
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
            params WorksheetContainer<Model.Fornecedor>[] containers)
        {
            descricao.Report("Iniciando exportação em Excel de Fornecedor");
            isIndeterminada.Report(true);

            WorksheetContainer<Model.Fornecedor> wscontainer = containers[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            isIndeterminada.Report(false);
            double incrementoProgresso = 100.0 / lista.Count;
            for (int i = 0; i < lista.Count; i++)
            {
                descricao.Report($"Escrevendo fornecedor {i + 1} de {lista.Count}");
                worksheet.Cells[i + 2, Model.Fornecedor.Colunas.Cnpj] = lista[i].Cnpj;
                worksheet.Cells[i + 2, Model.Fornecedor.Colunas.Nome] = lista[i].Nome;
                worksheet.Cells[i + 2, Model.Fornecedor.Colunas.NomeFantasia] = lista[i].Fantasia;
                worksheet.Cells[i + 2, Model.Fornecedor.Colunas.Email] = lista[i].Email;
                worksheet.Cells[i + 2, Model.Fornecedor.Colunas.Telefone] = lista[i].Telefone;
                valor.Report(incrementoProgresso);
            }
        }
    }
}
