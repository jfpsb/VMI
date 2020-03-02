using Newtonsoft.Json;
using NHibernate;
using System;
using System.IO;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;

namespace VandaModaIntimaWpf.Sincronizacao
{
    public static class OperacoesDatabaseLogFile<E> where E : class, IModel
    {
        private static readonly string Diretorio = "DatabaseLog";

        public static void EscreverJson(string operacao, E entidade)
        {
            try
            {
                if (!Directory.Exists($"{Diretorio}"))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory($"{Diretorio}");
                    directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                DateTime LastWriteTime = DateTime.Now;
                DatabaseLogFile<E> databaseLogFile = new DatabaseLogFile<E>() { OperacaoMySQL = operacao, Entidade = entidade, LastWriteTime = LastWriteTime };

                string json = JsonConvert.SerializeObject(databaseLogFile, Formatting.Indented);

                File.WriteAllText(Path.Combine(Diretorio, $"{databaseLogFile.GetClassName()} {databaseLogFile.GetIdentifier()}.json"), json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DeletaArquivo(string fileName)
        {
            File.Delete(Path.Combine(Diretorio, fileName));
            Console.WriteLine($"Deletado: {fileName}");
        }

        /// <summary>
        /// Apaga todos os LOGS e recria usando os dados presentes atualmente no banco de dados
        /// </summary>
        public async static void ResetaLogs()
        {
            Directory.Delete(Diretorio, true);

            ISession session = SessionProvider.GetSession("teste");

            DAOContagem dAOContagem = new DAOContagem(session);
            DAOContagemProduto dAOContagemProduto = new DAOContagemProduto(session);
            DAOFornecedor dAOFornecedor = new DAOFornecedor(session);
            DAOLoja dAOLoja = new DAOLoja(session);
            DAOMarca dAOMarca = new DAOMarca(session);
            DAOOperadoraCartao dAOOperadoraCartao = new DAOOperadoraCartao(session);
            DAOProduto dAOProduto = new DAOProduto(session);
            DAORecebimentoCartao dAORecebimentoCartao = new DAORecebimentoCartao(session);
            DAOTipoContagem dAOTipoContagem = new DAOTipoContagem(session);

            foreach (Contagem contagem in await dAOContagem.Listar())
            {
                OperacoesDatabaseLogFile<Contagem>.EscreverJson("INSERT", contagem);
            }

            foreach (ContagemProduto contagemProduto in await dAOContagemProduto.Listar())
            {
                OperacoesDatabaseLogFile<ContagemProduto>.EscreverJson("INSERT", contagemProduto);
            }

            foreach (Fornecedor fornecedor in await dAOFornecedor.Listar())
            {
                OperacoesDatabaseLogFile<Fornecedor>.EscreverJson("INSERT", fornecedor);
            }

            foreach (Loja Loja in await dAOLoja.Listar())
            {
                OperacoesDatabaseLogFile<Loja>.EscreverJson("INSERT", Loja);
            }

            foreach (Marca Marca in await dAOMarca.Listar())
            {
                OperacoesDatabaseLogFile<Marca>.EscreverJson("INSERT", Marca);
            }

            foreach (OperadoraCartao OperadoraCartao in await dAOOperadoraCartao.Listar())
            {
                OperacoesDatabaseLogFile<OperadoraCartao>.EscreverJson("INSERT", OperadoraCartao);
            }

            foreach (Produto Produto in await dAOProduto.Listar())
            {
                OperacoesDatabaseLogFile<Produto>.EscreverJson("INSERT", Produto);
            }

            foreach (RecebimentoCartao RecebimentoCartao in await dAORecebimentoCartao.Listar())
            {
                OperacoesDatabaseLogFile<RecebimentoCartao>.EscreverJson("INSERT", RecebimentoCartao);
            }

            foreach (TipoContagem tipoContagem in await dAOTipoContagem.Listar())
            {
                OperacoesDatabaseLogFile<TipoContagem>.EscreverJson("INSERT", tipoContagem);
            }

            SessionProvider.FechaSession("teste");
        }
    }
}
