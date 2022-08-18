using System.Collections.Generic;
using System.Globalization;
using VandaModaIntimaWpf.View.Produto.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for TelaRelatorioProduto.xaml
    /// </summary>
    public partial class TelaRelatorioProduto : System.Windows.Window
    {
        public TelaRelatorioProduto()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioProduto(IList<Model.Produto> produtos)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();

            ProdutoDataSet produtoDataSet = new ProdutoDataSet();

            foreach (var produto in produtos)
            {
                var prow = produtoDataSet.Produto.NewProdutoRow();

                if (produto.Fornecedor != null)
                {
                    prow.fornecedor_nome = produto.Fornecedor.Nome;
                }
                else
                {
                    prow.fornecedor_nome = "NÃO POSSUI";
                }

                if (produto.Marca != null)
                {
                    prow.marca_nome = produto.Marca.Nome;
                }
                else
                {
                    prow.marca_nome = "NÃO POSSUI";
                }

                if (!string.IsNullOrEmpty(produto.Ncm))
                {
                    prow.ncm = produto.Ncm;
                }
                else
                {
                    prow.ncm = "NÃO POSSUI";
                }

                prow.cod_barra = produto.CodBarra;
                prow.descricao = produto.Descricao;

                produtoDataSet.Produto.AddProdutoRow(prow);
            }

            var report = new RelatorioProduto();
            report.SetDataSource(produtoDataSet);
            //ProdutoReport.ViewerCore.ReportSource = report;
        }
    }
}
