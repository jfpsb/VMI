using Microsoft.Reporting.WinForms;

namespace VandaModaIntimaWpf.Util
{
    public class ReportViewerUtil
    {
        /// <summary>
        /// Configura o ReportViewer.
        /// </summary>
        /// <param name="reportViewer">Objeto ReportViewer a ser configurado</param>
        /// <param name="reportEmbeddedResource">Caminho do arquivo .rdlc em forma de namespaces. Arquivo precisa ser configurado como Embedded Resource nas suas propriedades</param>
        /// <param name="reportDataSource">DataSource principal do relátorio</param>
        /// <param name="subreportProcessing">Realiza o processamento caso haja SubReports nesse relatório</param>
        public static void ConfiguraReportViewer(ReportViewer reportViewer, string reportEmbeddedResource, ReportDataSource reportDataSource, SubreportProcessingEventHandler subreportProcessing = null)
        {
            reportViewer.Reset();
            reportViewer.LocalReport.ReportEmbeddedResource = reportEmbeddedResource;
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            if (subreportProcessing != null)
                reportViewer.LocalReport.SubreportProcessing += subreportProcessing;
            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
            reportViewer.ZoomMode = ZoomMode.Percent;
            reportViewer.ZoomPercent = 100;
            reportViewer.LocalReport.Refresh();
            reportViewer.RefreshReport();
        }
    }
}
