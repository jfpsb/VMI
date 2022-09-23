using Microsoft.Reporting.WinForms;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.View.Interfaces
{
    public interface IConfiguraReportViewer
    {
        /// <summary>
        /// Configura relatório usando ReportDataSource apontado.
        /// </summary>
        /// <param name="relatorio">Relatório a ser configurado</param>
        /// <param name="reportDataSource">Dados do relatório</param>
        /// <param name="embeddedPath">Caminho do relatório na aplicação (inclui nome do Assembly, separado por ponto)</param>
        /// <returns></returns>
        void Configurar(ReportViewer relatorio,
            ReportDataSource reportDataSource,
            string embeddedPath);
        Task<ReportDataSource> ConfigurarReportDataSource(params object[] fonte);
    }
}
