using NHibernate;
using System;
using System.Windows;
using VandaModaIntimaWpf.View.Interfaces;

namespace VandaModaIntimaWpf.View.FolhaPagamento
{
    /// <summary>
    /// Interaction logic for TelaRelatorioFolha.xaml
    /// </summary>
    public partial class TelaRelatorioFolha : Window
    {
        private IConfiguraReportViewer configuraReport;
        private Model.FolhaPagamento _folha;
        public TelaRelatorioFolha()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioFolha(ISession session, Model.FolhaPagamento FolhaPagamento)
        {
            _folha = FolhaPagamento;
            configuraReport = new ConfiguraReportViewerFolha(session);
            InitializeComponent();
        }

        private async void FolhaReportViewer_Load(object sender, EventArgs e)
        {
            var reportDataSource = await configuraReport.ConfigurarReportDataSource(_folha);
            configuraReport.Configurar(FolhaReportViewer, reportDataSource, "VandaModaIntimaWpf.View.FolhaPagamento.Relatorios.RelatorioFolhaPagamento.rdlc");
        }
    }
}
