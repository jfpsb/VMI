using System.Globalization;
using System.Windows;
using VandaModaIntimaWpf.View.EntradaDeMercadoria.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.EntradaDeMercadoria
{
    /// <summary>
    /// Interaction logic for TelaRelatorioEntradaMercadoria.xaml
    /// </summary>
    public partial class TelaRelatorioEntradaMercadoria : Window
    {
        public TelaRelatorioEntradaMercadoria()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioEntradaMercadoria(Model.EntradaDeMercadoria entrada)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();

            EntradaMercadoriaDataSet entradaMercadoriaDataSet = new EntradaMercadoriaDataSet();
            EntradaMercadoriaProdutoGradeDataSet entradaMercadoriaProdutoGradeDataSet = new EntradaMercadoriaProdutoGradeDataSet();

            var report = new RelatorioEntradaMercadoria();

            foreach (var empg in entrada.Entradas)
            {
                var empgRow = entradaMercadoriaProdutoGradeDataSet.entradamercadoria_produtograde.Newentradamercadoria_produtogradeRow();
                if (empg.ProdutoGrade != null)
                {
                    empgRow.codbarra_grade = empg.ProdutoGrade.CodBarra;
                    empgRow.codbarraalt_grade = empg.ProdutoGrade.CodBarraAlternativo;
                }
                empgRow.descricao_produto = empg.ProdutoDescricao;
                empgRow.descricao_grade = empg.GradeDescricao;
                empgRow.preco_grade = empg.GradePreco.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
                empgRow.quantidade = empg.Quantidade.ToString();

                entradaMercadoriaProdutoGradeDataSet.entradamercadoria_produtograde.Addentradamercadoria_produtogradeRow(empgRow);
            }

            var eRow = entradaMercadoriaDataSet.entradamercadoria.NewentradamercadoriaRow();
            eRow.id = entrada.Id.ToString();
            eRow.loja_nome = entrada.Loja.Nome;
            eRow.data = entrada.Data.ToString("dd/MM/yyyy HH:mm:ss");

            entradaMercadoriaDataSet.entradamercadoria.AddentradamercadoriaRow(eRow);

            report.Subreports[0].SetDataSource(entradaMercadoriaProdutoGradeDataSet);
            report.SetDataSource(entradaMercadoriaDataSet);

            //EntradaReport.ViewerCore.EnableDrillDown = false;
            //EntradaReport.ViewerCore.ReportSource = report;
        }

        private void ReportV_Load(object sender, System.EventArgs e)
        {

        }
    }
}
