using NHibernate;
using SincronizacaoBD.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SincronizacaoBD.Sincronizacao
{
    public static class SincronizacaoRemota
    {
        private static bool EmExecucao = false;
        private static readonly string Caminho = "LastUpdate.txt";
        private static ISession sessionLocal;
        private static ISession sessionSync;

        public static void Sincronizar(Action<string> AdicionaTexto)
        {
            if (!EmExecucao)
            {
                try
                {
                    EmExecucao = true;

                    DateTime inicioSincronizacao = DateTime.Now;
                    AdicionaTexto($"Iniciando Sincronização às {inicioSincronizacao.ToString("dd/MM/yyyy HH:mm:ss")}\n");

                    DateTime lastUpdate = new DateTime(2010, 1, 1, 12, 0, 0);

                    if (File.Exists(Caminho))
                    {
                        using (StreamReader streamReader = File.OpenText(Caminho))
                        {
                            string dataString = streamReader.ReadToEnd();
                            lastUpdate = DateTime.Parse(dataString);
                        }
                    }

                    sessionSync = SessionSyncProvider.GetSessionSync();

                    DAOSync<Fornecedor> daoFornecedorSync = new DAOSync<Fornecedor>(sessionSync);
                    DAOSync<Loja> daoLojaSync = new DAOSync<Loja>(sessionSync);
                    DAOSync<Marca> daoMarcaSync = new DAOSync<Marca>(sessionSync);
                    DAOSync<OperadoraCartao> daoOperadoraCartaoSync = new DAOSync<OperadoraCartao>(sessionSync);
                    DAOSync<Produto> daoProdutoSync = new DAOSync<Produto>(sessionSync);
                    DAOSync<RecebimentoCartao> daoRecebimentoCartaoSync = new DAOSync<RecebimentoCartao>(sessionSync);

                    AdicionaTexto($"{DataHoraAtual()}: Consultando Banco de Dados Remoto\n");

                    IList<Fornecedor> fornecedores = daoFornecedorSync.ListarLastUpdate(lastUpdate);
                    IList<Loja> lojas = daoLojaSync.ListarLastUpdate(lastUpdate, "Matriz");
                    IList<Marca> marcas = daoMarcaSync.ListarLastUpdate(lastUpdate);
                    IList<OperadoraCartao> operadoras = daoOperadoraCartaoSync.ListarLastUpdate(lastUpdate, "IdentificadoresBanco");
                    IList<Produto> produtos = daoProdutoSync.ListarLastUpdate(lastUpdate, "Codigos");
                    IList<RecebimentoCartao> recebimentos = daoRecebimentoCartaoSync.ListarLastUpdate(lastUpdate, "Loja", "OperadoraCartao");

                    SessionSyncProvider.FechaSession(sessionSync);

                    sessionLocal = SessionSyncProvider.GetSession("Sincronizacao");

                    DAOSync<Fornecedor> daoFornecedorLocal = new DAOSync<Fornecedor>(sessionLocal);
                    DAOSync<Loja> daoLojaLocal = new DAOSync<Loja>(sessionLocal);
                    DAOSync<Marca> daoMarcaLocal = new DAOSync<Marca>(sessionLocal);
                    DAOSync<OperadoraCartao> daoOperadoraCartaoLocal = new DAOSync<OperadoraCartao>(sessionLocal);
                    DAOSync<Produto> daoProdutoLocal = new DAOSync<Produto>(sessionLocal);
                    DAOSync<RecebimentoCartao> daoRecebimentoCartaoLocal = new DAOSync<RecebimentoCartao>(sessionLocal);

                    AdicionaTexto($"{DataHoraAtual()}: Inserindo Em Banco de Dados Local Registros Recuperados do Banco de Dados Remoto\n");

                    if (fornecedores.Count > 0)
                    {
                        daoFornecedorLocal.InserirOuAtualizar(fornecedores);
                    }

                    if (lojas.Count > 0)
                    {
                        daoLojaLocal.InserirOuAtualizar(lojas);
                    }

                    if (marcas.Count > 0)
                    {
                        daoMarcaLocal.InserirOuAtualizar(marcas);
                    }

                    if (operadoras.Count > 0)
                    {
                        daoOperadoraCartaoLocal.InserirOuAtualizar(operadoras);
                    }

                    if (produtos.Count > 0)
                    {
                        daoProdutoLocal.InserirOuAtualizar(produtos);
                    }

                    if (recebimentos.Count > 0)
                    {
                        daoRecebimentoCartaoLocal.InserirOuAtualizar(recebimentos);
                    }

                    SessionSyncProvider.FechaSession(sessionLocal);

                    AdicionaTexto($"{DataHoraAtual()}: Lendo Arquivos Com Registros Das Mudanças Locais\n");

                    IList<EntidadeMySQL<Fornecedor>> fornecedoresLocais = ArquivoEntidade<Fornecedor>.LerDeBinario();
                    IList<EntidadeMySQL<Loja>> lojasLocais = ArquivoEntidade<Loja>.LerDeBinario();
                    IList<EntidadeMySQL<Marca>> marcasLocais = ArquivoEntidade<Marca>.LerDeBinario();
                    IList<EntidadeMySQL<OperadoraCartao>> operadorasLocais = ArquivoEntidade<OperadoraCartao>.LerDeBinario();
                    IList<EntidadeMySQL<Produto>> produtosLocais = ArquivoEntidade<Produto>.LerDeBinario();
                    IList<EntidadeMySQL<RecebimentoCartao>> recebimentosLocais = ArquivoEntidade<RecebimentoCartao>.LerDeBinario();

                    sessionSync = SessionSyncProvider.GetSessionSync();

                    daoFornecedorSync = new DAOSync<Fornecedor>(sessionSync);
                    daoLojaSync = new DAOSync<Loja>(sessionSync);
                    daoMarcaSync = new DAOSync<Marca>(sessionSync);
                    daoOperadoraCartaoSync = new DAOSync<OperadoraCartao>(sessionSync);
                    daoProdutoSync = new DAOSync<Produto>(sessionSync);
                    daoRecebimentoCartaoSync = new DAOSync<RecebimentoCartao>(sessionSync);

                    AdicionaTexto($"{DataHoraAtual()}: Inserindo em Banco de Dados Remoto Dados Locais\n");

                    if (fornecedoresLocais?.Count > 0)
                    {
                        IList<Fornecedor> listaInsertUpdate = fornecedoresLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<Fornecedor> listaDelete = fornecedoresLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        if (listaInsertUpdate.Count > 0)
                            daoFornecedorSync.InserirOuAtualizar(listaInsertUpdate);

                        if (listaDelete.Count > 0)
                            daoFornecedorSync.Deletar(listaDelete);
                    }

                    if (lojasLocais?.Count > 0)
                    {
                        IList<Loja> listaInsertUpdate = lojasLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<Loja> listaDelete = lojasLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        if (listaInsertUpdate.Count > 0)
                            daoLojaSync.InserirOuAtualizar(listaInsertUpdate);

                        if (listaDelete.Count > 0)
                            daoLojaSync.Deletar(listaDelete);
                    }

                    if (marcasLocais?.Count > 0)
                    {
                        IList<Marca> listaInsertUpdate = marcasLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<Marca> listaDelete = marcasLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        if (listaInsertUpdate.Count > 0)
                            daoMarcaSync.InserirOuAtualizar(listaInsertUpdate);

                        if (listaDelete.Count > 0)
                            daoMarcaSync.Deletar(listaDelete);
                    }

                    if (operadorasLocais?.Count > 0)
                    {
                        IList<OperadoraCartao> listaInsertUpdate = operadorasLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<OperadoraCartao> listaDelete = operadorasLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        if (listaInsertUpdate.Count > 0)
                            daoOperadoraCartaoSync.InserirOuAtualizar(listaInsertUpdate);

                        if (listaDelete.Count > 0)
                            daoOperadoraCartaoSync.Deletar(listaDelete);
                    }

                    if (produtosLocais?.Count > 0)
                    {
                        IList<Produto> listaInsertUpdate = produtosLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<Produto> listaDelete = produtosLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        if (listaInsertUpdate.Count > 0)
                            daoProdutoSync.InserirOuAtualizar(listaInsertUpdate);

                        if (listaDelete.Count > 0)
                            daoProdutoSync.Deletar(listaDelete);
                    }

                    if (recebimentosLocais?.Count > 0)
                    {
                        IList<RecebimentoCartao> listaInsertUpdate = recebimentosLocais.Where(
                            w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        IList<RecebimentoCartao> listaDelete = recebimentosLocais.Where(
                            w => w.OperacaoMySql.Equals("DELETE")).
                            Select(sm => sm.EntidadeSalva).ToList();

                        if (listaInsertUpdate.Count > 0)
                            daoRecebimentoCartaoSync.InserirOuAtualizar(listaInsertUpdate);

                        if (listaDelete.Count > 0)
                            daoRecebimentoCartaoSync.Deletar(listaDelete);
                    }

                    SessionSyncProvider.FechaSession(sessionSync);

                    using (StreamWriter streamWriter = new StreamWriter(Caminho, false))
                    {
                        streamWriter.WriteLine(inicioSincronizacao.ToString("yyyy-MM-dd HH:mm:ss"));
                    }

                    EsvaziaDiretorios();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    SessionSyncProvider.FechaSession(sessionSync);
                    SessionSyncProvider.FechaSession(sessionLocal);
                    EmExecucao = false;
                    AdicionaTexto($"{DataHoraAtual()}: Sincronização Finalizada\n");
                }
            }
        }
        private static void EsvaziaDiretorios()
        {
            foreach (var dir in new DirectoryInfo("EntidadesSalvas").GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private static string DataHoraAtual()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
