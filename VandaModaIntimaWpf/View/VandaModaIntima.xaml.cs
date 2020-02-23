using NHibernate;
using System.Windows;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model.DAO.MySQL;
using VandaModaIntimaWpf.Sincronizacao;

namespace VandaModaIntimaWpf
{
    public partial class VandaModaIntima : Window
    {
        public VandaModaIntima()
        {
            InitializeComponent();

            SessionProvider.MySessionFactory = SessionProvider.BuildSessionFactory();

            //string JsonText = File.ReadAllText(Path.Combine("DatabaseLog", "Produto 13704.json"));
            //var databaseLogFile = JsonConvert.DeserializeObject<DatabaseLogFile<Produto>>(JsonText, new ProdutoJsonConverter());

            //mainWindow = new MainWindow();
            //mainWindow.Show();

            //Teste();
        }

        private async void Teste()
        {
            ISession session = SessionProvider.GetSession("teste");

            DAOFornecedor dAOFornecedor = new DAOFornecedor(session);
            DAOLoja dAOLoja = new DAOLoja(session);
            DAOMarca dAOMarca = new DAOMarca(session);
            DAOOperadoraCartao dAOOperadoraCartao = new DAOOperadoraCartao(session);
            DAOProduto dAOProduto = new DAOProduto(session);
            DAORecebimentoCartao dAORecebimentoCartao = new DAORecebimentoCartao(session);

            foreach (Model.Fornecedor fornecedor in await dAOFornecedor.Listar())
            {
                OperacoesDatabaseLogFile<Model.Fornecedor>.EscreverJson("INSERT", fornecedor);
            }

            foreach (Model.Loja Loja in await dAOLoja.Listar())
            {
                OperacoesDatabaseLogFile<Model.Loja>.EscreverJson("INSERT", Loja);
            }

            foreach (Model.Marca Marca in await dAOMarca.Listar())
            {
                OperacoesDatabaseLogFile<Model.Marca>.EscreverJson("INSERT", Marca);
            }

            foreach (Model.OperadoraCartao OperadoraCartao in await dAOOperadoraCartao.Listar())
            {
                OperacoesDatabaseLogFile<Model.OperadoraCartao>.EscreverJson("INSERT", OperadoraCartao);
            }

            foreach (Model.Produto Produto in await dAOProduto.Listar())
            {
                OperacoesDatabaseLogFile<Model.Produto>.EscreverJson("INSERT", Produto);
            }

            foreach (Model.RecebimentoCartao RecebimentoCartao in await dAORecebimentoCartao.Listar())
            {
                OperacoesDatabaseLogFile<Model.RecebimentoCartao>.EscreverJson("INSERT", RecebimentoCartao);
            }

            SessionProvider.FechaSession("teste");
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void TelaPrincipal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionProvider.FechaConexoes();
        }

        private void TelaPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
