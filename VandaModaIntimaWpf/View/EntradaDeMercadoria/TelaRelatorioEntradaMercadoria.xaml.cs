using Microsoft.Reporting.WinForms;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Windows;
using VandaModaIntimaWpf.Util;
using VandaModaIntimaWpf.View.EntradaDeMercadoria.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.EntradaDeMercadoria
{
    /// <summary>
    /// Interaction logic for TelaRelatorioEntradaMercadoria.xaml
    /// </summary>
    public partial class TelaRelatorioEntradaMercadoria : Window
    {
        private Model.EntradaDeMercadoria _entrada;
        public TelaRelatorioEntradaMercadoria()
        {
            //System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioEntradaMercadoria(Model.EntradaDeMercadoria entrada)
        {
            InitializeComponent();
            _entrada = entrada;
        }

        private void ReportV_Load(object sender, System.EventArgs e)
        {
            ReportDataSource reportDataSource = new ReportDataSource("DataSetEntradaMercadoria", GetEntradaMercadoriaDataTable(_entrada));
            ReportViewerUtil.ConfiguraReportViewer(EntradaMercadoriaReportViewer,
                "VandaModaIntimaWpf.View.EntradaDeMercadoria.Relatorios.EntradaMercadoria.rdlc",
                reportDataSource,
                LocalReport_SubreportProcessing);
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            ReportDataSource reportDataSource = new ReportDataSource("DataSetEntradaMercadoriaProdutoGrade", GetEntradaMercadoriaProdutoGradeDataTable(_entrada));
            e.DataSources.Add(reportDataSource);
        }

        private DataTable GetEntradaMercadoriaDataTable(Model.EntradaDeMercadoria entrada)
        {
            EntradaMercadoriaDataSet entradaMercadoriaDataSet = new EntradaMercadoriaDataSet();

            var eRow = entradaMercadoriaDataSet.entradamercadoria.NewentradamercadoriaRow();
            eRow.id = entrada.Id.ToString();
            eRow.loja = entrada.Loja.Nome;
            eRow.data = entrada.Data.ToString("dd/MM/yyyy HH:mm:ss");
            entradaMercadoriaDataSet.entradamercadoria.AddentradamercadoriaRow(eRow);

            return entradaMercadoriaDataSet.Tables[0];
        }

        private DataTable GetEntradaMercadoriaProdutoGradeDataTable(Model.EntradaDeMercadoria entrada)
        {
            EntradaMercadoriaProdutoGradeDataSet entradaMercadoriaProdutoGradeDataSet = new EntradaMercadoriaProdutoGradeDataSet();

            foreach (var empg in entrada.Entradas)
            {
                var empgRow = entradaMercadoriaProdutoGradeDataSet.entradamercadoria_produtograde.Newentradamercadoria_produtogradeRow();
                if (empg.ProdutoGrade != null)
                {
                    empgRow.cod_barra = empg.ProdutoGrade.CodBarra;
                    empgRow.cod_barra_alternativo = empg.ProdutoGrade.CodBarraAlternativo;
                }
                empgRow.produto_descricao = empg.ProdutoDescricao;
                empgRow.grade_descricao = empg.GradeDescricao;
                empgRow.grade_preco = empg.GradePreco.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                empgRow.quantidade = empg.Quantidade.ToString();
                empgRow.entradamercadoria = entrada.Id;

                entradaMercadoriaProdutoGradeDataSet.entradamercadoria_produtograde.Addentradamercadoria_produtogradeRow(empgRow);
            }

            return entradaMercadoriaProdutoGradeDataSet.Tables[0];
        }
    }
}
