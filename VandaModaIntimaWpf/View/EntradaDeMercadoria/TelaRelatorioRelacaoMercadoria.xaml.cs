using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
                    empgRow.codbarra_grade = empg.ProdutoGrade.CodBarra;
                    empgRow.codbarraalt_grade = empg.ProdutoGrade.CodBarraAlternativo;
                }
                empgRow.descricao_produto = empg.ProdutoDescricao;
                empgRow.descricao_grade = empg.GradeDescricao;
                empgRow.preco_grade = empg.GradePreco.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));

                entradaMercadoriaProdutoGradeDataSet.entradamercadoria_produtograde.Addentradamercadoria_produtogradeRow(empgRow);
            }

            var eRow = entradaMercadoriaDataSet.entradamercadoria.NewentradamercadoriaRow();
            eRow.id = entrada.Id.ToString();
            eRow.loja_nome = entrada.Loja.Nome;
            eRow.data = entrada.Data.ToString("dd/MM/yyyy HH:mm:ss");

            entradaMercadoriaDataSet.entradamercadoria.AddentradamercadoriaRow(eRow);

            report.Subreports[0].SetDataSource(entradaMercadoriaProdutoGradeDataSet);
            report.SetDataSource(entradaMercadoriaDataSet);

            RelacaoReport.ViewerCore.EnableDrillDown = false;
            RelacaoReport.ViewerCore.ReportSource = report;
        }
    }
}
