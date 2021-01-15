using NHibernate;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.View.Produto.Relatorios;
using VandaModaIntimaWpf.ViewModel.DataSets;

namespace VandaModaIntimaWpf.View.Produto
{
    /// <summary>
    /// Interaction logic for TelaRelatorioProduto.xaml
    /// </summary>
    public partial class TelaRelatorioProduto : System.Windows.Window
    {
        private DAOFornecedor daoFornecedor;
        private DAOMarca daoMarca;
        public TelaRelatorioProduto()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        public TelaRelatorioProduto(IList<Model.Produto> produtos)
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;

            InitializeComponent();

            ISession session = SessionProvider.GetSession();
            daoFornecedor = new DAOFornecedor(session);
            daoMarca = new DAOMarca(session);

            var fornecedores = GetFornecedores().Result;
            var marcas = GetMarcas().Result;

            fornecedores.Add(new Model.Fornecedor("NÃO POSSUI"));
            marcas.Add(new Model.Marca("NÃO POSSUI"));

            ProdutoDataSet produtoDataSet = new ProdutoDataSet();

            foreach (var f in fornecedores)
            {
                var frow = produtoDataSet.Fornecedor.NewFornecedorRow();
                frow.cnpj = f.Cnpj;
                frow.nome = f.Nome;
                produtoDataSet.Fornecedor.AddFornecedorRow(frow);
            }            

            foreach (var m in marcas)
            {
                var mrow = produtoDataSet.Marca.NewMarcaRow();
                mrow.nome = m.Nome;
                produtoDataSet.Marca.AddMarcaRow(mrow);
            }

            foreach (var produto in produtos)
            {
                var prow = produtoDataSet.Produto.NewProdutoRow();

                if (produto.Fornecedor != null)
                {
                    prow.fornecedor = produto.Fornecedor.Cnpj;
                }
                else
                {
                    prow.fornecedor = "0";
                }

                if (produto.Marca != null)
                {
                    prow.marca = produto.Marca.Nome;
                }
                else
                {
                    prow.marca = "NÃO POSSUI";
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
                prow.preco = produto.Preco.ToString();

                produtoDataSet.Produto.AddProdutoRow(prow);
            }

            SessionProvider.FechaSession(session);
            var report = new RelatorioProduto();
            report.SetDataSource(produtoDataSet);
            ProdutoReport.ViewerCore.ReportSource = report;
        }

        private async Task<IList<Model.Fornecedor>> GetFornecedores()
        {
            return await daoFornecedor.Listar<Model.Fornecedor>();
        }

        private async Task<IList<Model.Marca>> GetMarcas()
        {
            return await daoMarca.Listar<Model.Marca>();
        }
    }
}
