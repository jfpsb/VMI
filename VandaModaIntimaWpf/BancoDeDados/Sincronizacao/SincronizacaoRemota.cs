using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    public static class SincronizacaoRemota
    {
        private static bool EmExecucao = false;
        private static readonly string Caminho = "LastUpdate.txt";
        private static ISession sessionLocal;
        private static ISession sessionSync;

        public async static Task Sincronizar()
        {
            if (!EmExecucao)
            {
                try
                {
                    EmExecucao = true;

                    DateTime lastUpdate = new DateTime(2010, 1, 1, 12, 0, 0);

                    if (File.Exists(Caminho))
                    {
                        using (StreamReader streamReader = File.OpenText(Caminho))
                        {
                            string dataString = streamReader.ReadToEnd();
                            lastUpdate = DateTime.Parse(dataString);
                        }
                    }

                    sessionSync = SessionProvider.GetSessionSync();

                    DAOSync<Fornecedor> daoFornecedorSync = new DAOSync<Fornecedor>(sessionSync);
                    DAOSync<Loja> daoLojaSync = new DAOSync<Loja>(sessionSync);
                    DAOSync<Marca> daoMarcaSync = new DAOSync<Marca>(sessionSync);
                    DAOSync<OperadoraCartao> daoOperadoraCartaoSync = new DAOSync<OperadoraCartao>(sessionSync);
                    DAOSync<Produto> daoProdutoSync = new DAOSync<Produto>(sessionSync);
                    DAOSync<RecebimentoCartao> daoRecebimentoCartaoSync = new DAOSync<RecebimentoCartao>(sessionSync);

                    IList<Fornecedor> fornecedores = await daoFornecedorSync.ListarLastUpdate(lastUpdate);
                    IList<Loja> lojas = await daoLojaSync.ListarLastUpdate(lastUpdate, "Matriz");
                    IList<Marca> marcas = await daoMarcaSync.ListarLastUpdate(lastUpdate);
                    IList<OperadoraCartao> operadoras = await daoOperadoraCartaoSync.ListarLastUpdate(lastUpdate, "IdentificadoresBanco");
                    IList<Produto> produtos = await daoProdutoSync.ListarLastUpdate(lastUpdate, "Codigos");
                    IList<RecebimentoCartao> recebimentos = await daoRecebimentoCartaoSync.ListarLastUpdate(lastUpdate, "Loja", "OperadoraCartao");

                    SessionProvider.FechaSessionSync(sessionSync);

                    sessionLocal = SessionProvider.GetSession("Sincronizacao");

                    DAOSync<Fornecedor> daoFornecedorLocal = new DAOSync<Fornecedor>(sessionLocal);
                    DAOSync<Loja> daoLojaLocal = new DAOSync<Loja>(sessionLocal);
                    DAOSync<Marca> daoMarcaLocal = new DAOSync<Marca>(sessionLocal);
                    DAOSync<OperadoraCartao> daoOperadoraCartaoLocal = new DAOSync<OperadoraCartao>(sessionLocal);
                    DAOSync<Produto> daoProdutoLocal = new DAOSync<Produto>(sessionLocal);
                    DAOSync<RecebimentoCartao> daoRecebimentoCartaoLocal = new DAOSync<RecebimentoCartao>(sessionLocal);

                    if (fornecedores.Count > 0)
                    {
                        await daoFornecedorLocal.InserirOuAtualizar(fornecedores);
                    }

                    if (lojas.Count > 0)
                    {
                        await daoLojaLocal.InserirOuAtualizar(lojas);
                    }

                    if (marcas.Count > 0)
                    {
                        await daoMarcaLocal.InserirOuAtualizar(marcas);
                    }

                    if (operadoras.Count > 0)
                    {
                        await daoOperadoraCartaoLocal.InserirOuAtualizar(operadoras);
                    }

                    if (produtos.Count > 0)
                    {
                        await daoProdutoLocal.InserirOuAtualizar(produtos);
                    }

                    if (recebimentos.Count > 0)
                    {
                        await daoRecebimentoCartaoLocal.InserirOuAtualizar(recebimentos);
                    }

                    //SessionProvider.FechaSession("Sincronizacao");

                    IList<EntidadeMySQL<Fornecedor>> fornecedoresLocais = new ArquivoEntidade<Fornecedor>().LerDeBinario();
                    IList<EntidadeMySQL<Loja>> lojasLocais = new ArquivoEntidade<Loja>().LerDeBinario();
                    IList<EntidadeMySQL<Marca>> marcasLocais = new ArquivoEntidade<Marca>().LerDeBinario();
                    IList<EntidadeMySQL<OperadoraCartao>> operadorasLocais = new ArquivoEntidade<OperadoraCartao>().LerDeBinario();
                    IList<EntidadeMySQL<Produto>> produtosLocais = new ArquivoEntidade<Produto>().LerDeBinario();
                    IList<EntidadeMySQL<RecebimentoCartao>> recebimentosLocais = new ArquivoEntidade<RecebimentoCartao>().LerDeBinario();

                    sessionSync = SessionProvider.GetSessionSync();

                    daoFornecedorSync = new DAOSync<Fornecedor>(sessionSync);
                    daoLojaSync = new DAOSync<Loja>(sessionSync);
                    daoMarcaSync = new DAOSync<Marca>(sessionSync);
                    daoOperadoraCartaoSync = new DAOSync<OperadoraCartao>(sessionSync);
                    daoProdutoSync = new DAOSync<Produto>(sessionSync);
                    daoRecebimentoCartaoSync = new DAOSync<RecebimentoCartao>(sessionSync);

                    if (fornecedoresLocais?.Count > 0)
                    {
                        IList<Fornecedor> listaInsertUpdate = fornecedoresLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<Fornecedor> listaDelete = fornecedoresLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        await daoFornecedorSync.InserirOuAtualizar(listaInsertUpdate);
                        await daoFornecedorSync.Deletar(listaDelete);
                    }

                    if (lojasLocais?.Count > 0)
                    {
                        IList<Loja> listaInsertUpdate = lojasLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<Loja> listaDelete = lojasLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        await daoLojaSync.InserirOuAtualizar(listaInsertUpdate);
                        await daoLojaSync.Deletar(listaDelete);
                    }

                    if (marcasLocais?.Count > 0)
                    {
                        IList<Marca> listaInsertUpdate = marcasLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<Marca> listaDelete = marcasLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        await daoMarcaSync.InserirOuAtualizar(listaInsertUpdate);
                        await daoMarcaSync.Deletar(listaDelete);
                    }

                    if (operadorasLocais?.Count > 0)
                    {
                        IList<OperadoraCartao> listaInsertUpdate = operadorasLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<OperadoraCartao> listaDelete = operadorasLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        await daoOperadoraCartaoSync.InserirOuAtualizar(listaInsertUpdate);
                        await daoOperadoraCartaoSync.Deletar(listaDelete);
                    }

                    if (produtosLocais?.Count > 0)
                    {
                        IList<Produto> listaInsertUpdate = produtosLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<Produto> listaDelete = produtosLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        await daoProdutoSync.InserirOuAtualizar(listaInsertUpdate);
                        await daoProdutoSync.Deletar(listaDelete);
                    }

                    if (recebimentosLocais?.Count > 0)
                    {
                        IList<RecebimentoCartao> listaInsertUpdate = recebimentosLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<RecebimentoCartao> listaDelete = recebimentosLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        await daoRecebimentoCartaoSync.InserirOuAtualizar(listaInsertUpdate);
                        await daoRecebimentoCartaoSync.Deletar(listaDelete);
                    }

                    SessionProvider.FechaSessionSync(sessionSync);

                    using (StreamWriter streamWriter = new StreamWriter(Caminho, false))
                    {
                        streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                finally
                {
                    SessionProvider.FechaSessionSync(sessionSync);
                    SessionProvider.FechaSession("Sincronizacao");
                    EmExecucao = false;
                }
            }
        }
    }
}
