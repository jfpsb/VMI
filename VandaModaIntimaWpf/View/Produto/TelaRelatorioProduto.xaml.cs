using System.Windows;
using VandaModaIntimaWpf.View.Produto.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for TelaRelatorioProduto.xaml
    /// </summary>
    public partial class TelaRelatorioProduto : Window
    {
        public TelaRelatorioProduto()
        {
            InitializeComponent();

            ProdutoDataSet produtoDataSet = new ProdutoDataSet();
            var row = produtoDataSet.Produto.NewProdutoRow();
            row.cod_barra = "12000";
            row.descricao = "TESTE";
            row.preco = "29.0";
            produtoDataSet.Produto.AddProdutoRow(row);

            var report = new RelatorioProduto();
            report.SetDataSource(produtoDataSet);
            ProdutoReport.ViewerCore.ReportSource = report;
        }
    }
}
