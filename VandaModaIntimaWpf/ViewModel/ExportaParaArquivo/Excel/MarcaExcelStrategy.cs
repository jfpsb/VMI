using Microsoft.Office.Interop.Excel;
using NHibernate;
using System;
using System.Threading;

namespace VandaModaIntimaWpf.ViewModel.ExportaParaArquivo.Excel
{
    class MarcaExcelStrategy : AExcelStrategy<Model.Marca>
    {
        private ISession _session;

        public MarcaExcelStrategy(ISession session)
        {
            _session = session;
        }

        public override void AutoFitColunas(Worksheet Worksheet)
        {
            Worksheet.Columns.AutoFit();
        }

        public override void EscreveDados(Workbook workbook,
            CancellationToken token,
            IProgress<string> descricao,
            IProgress<double> valor,
            IProgress<bool> isIndeterminada,
            params WorksheetContainer<Model.Marca>[] containers)
        {
            descricao.Report("Iniciando exportação em Excel de Marca");
            WorksheetContainer<Model.Marca> wscontainer = containers[0];
            var lista = wscontainer.Lista;
            var worksheet = workbook.Worksheets.Add();
            worksheet.Name = wscontainer.Nome;
            worksheet.Cells.Font.Size = wscontainer.TamanhoFonteGeral;

            double incrementoProgresso = 100.0 / lista.Count;
            for (int i = 0; i < lista.Count; i++)
            {
                token.ThrowIfCancellationRequested();
                descricao.Report($"Escrevendo marca {i + 1} de {lista.Count}");
                worksheet.Cells[i + 2, Model.Marca.Colunas.Nome] = lista[i].Nome;
                valor.Report(incrementoProgresso);
            }
        }
    }
}
