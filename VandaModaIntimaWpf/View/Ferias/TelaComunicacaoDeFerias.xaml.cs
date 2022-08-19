using Microsoft.Reporting.WinForms;
using System;
using System.Windows;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.Ferias.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.Ferias
{
    /// <summary>
    /// Interaction logic for TelaComunicacaoDeFerias.xaml
    /// </summary>
    public partial class TelaComunicacaoDeFerias : Window
    {
        private Model.Ferias _ferias;
        public TelaComunicacaoDeFerias()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
            //FeriasReport.Owner = this;
        }

        public TelaComunicacaoDeFerias(Model.Ferias ferias)
        {
            InitializeComponent();
            _ferias = ferias;
        }

        private void FeriasReportViewer_Load(object sender, EventArgs e)
        {
            FeriasDataSet feriasDataSet = new FeriasDataSet();
            var frow = feriasDataSet.ferias.NewferiasRow();

            frow.loja = Convert.ToUInt64(_ferias.Funcionario.Loja.Cnpj).ToString(@"00\.000\.000\/0000\-00");
            frow.nome_funcionario = _ferias.Funcionario.Nome;
            frow.cpf = Convert.ToUInt64(_ferias.Funcionario.Cpf).ToString(@"000\.000\.000\-00");
            frow.retorno = _ferias.Fim.AddDays(1).ToString("dd 'de' MMMM 'de' yyyy");
            frow.inicio = _ferias.Inicio.ToString("dd 'de' MMMM 'de' yyyy");
            frow.fim = _ferias.Fim.ToString("dd 'de' MMMM 'de' yyyy");
            frow.inicioaquisitivo = _ferias.InicioAquisitivo.ToString("dd 'de' MMMM 'de' yyyy");
            frow.fimaquisitivo = _ferias.FimAquisitivo.ToString("dd 'de' MMMM 'de' yyyy");

            feriasDataSet.ferias.AddferiasRow(frow);

            ReportDataSource reportDataSource = new ReportDataSource("DataSetFerias", feriasDataSet.Tables[0]);

            ReportViewerUtil.ConfiguraReportViewer(FeriasReportViewer,
                "VandaModaIntimaWpf.View.Ferias.Relatorios.RelatorioFerias.rdlc",
                reportDataSource,
                null);
        }
    }
}
