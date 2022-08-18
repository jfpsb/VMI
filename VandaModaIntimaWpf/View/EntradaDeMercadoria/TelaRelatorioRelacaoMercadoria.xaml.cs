using System.Globalization;
using System.Windows;
using VandaModaIntimaWpf.View.EntradaDeMercadoria.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.EntradaDeMercadoria
{
    /// <summary>
    /// Interaction logic for TelaRelatorioRelacaoMercadoria.xaml
    /// </summary>
    public partial class TelaRelatorioRelacaoMercadoria : Window
    {
        public TelaRelatorioRelacaoMercadoria()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioRelacaoMercadoria(Model.EntradaDeMercadoria entrada)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();

            EntradaMercadoriaDataSet entradaMercadoriaDataSet = new EntradaMercadoriaDataSet();
            EntradaMercadoriaProdutoGradeDataSet entradaMercadoriaProdutoGradeDataSet = new EntradaMercadoriaProdutoGradeDataSet();

            var report = new RelatorioRelacaoMercadoria();

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

                entradaMercadoriaProdutoGradeDataSet.entradamercadoria_produtograde.Addentradamercadoria_produtogradeRow(empgRow);
            }

            var eRow = entradaMercadoriaDataSet.entradamercadoria.NewentradamercadoriaRow();
            eRow.id = entrada.Id.ToString();
            eRow.loja = entrada.Loja.Nome;
            eRow.data = entrada.Data.ToString("dd/MM/yyyy HH:mm:ss");

            entradaMercadoriaDataSet.entradamercadoria.AddentradamercadoriaRow(eRow);

            report.Subreports[0].SetDataSource(entradaMercadoriaProdutoGradeDataSet);
            report.SetDataSource(entradaMercadoriaDataSet);

            //RelacaoReport.ViewerCore.EnableDrillDown = false;
            //RelacaoReport.ViewerCore.ReportSource = report;
        }
    }
}
