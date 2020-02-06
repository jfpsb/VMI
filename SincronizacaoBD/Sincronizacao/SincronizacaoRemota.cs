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

                    if (!File.Exists(Caminho))
                    {
                        sessionSync = SessionSyncProvider.GetSessionSync();

                        DAOSync<Fornecedor> daoFornecedorSync = new DAOSync<Fornecedor>(sessionSync);
                        DAOSync<Loja> daoLojaSync = new DAOSync<Loja>(sessionSync);
                        DAOSync<Marca> daoMarcaSync = new DAOSync<Marca>(sessionSync);
                        DAOSync<OperadoraCartao> daoOperadoraCartaoSync = new DAOSync<OperadoraCartao>(sessionSync);
                        DAOSync<Produto> daoProdutoSync = new DAOSync<Produto>(sessionSync);
                        DAOSync<RecebimentoCartao> daoRecebimentoCartaoSync = new DAOSync<RecebimentoCartao>(sessionSync);

                        AdicionaTexto($"{DataHoraAtual()}: Sincronizando pela primeira vez. Consultando Banco de Dados Remoto\n");

                        IList<Fornecedor> fornecedores = daoFornecedorSync.Listar();
                        IList<Loja> lojas = daoLojaSync.Listar();
                        IList<Marca> marcas = daoMarcaSync.Listar();
                        IList<OperadoraCartao> operadoras = daoOperadoraCartaoSync.Listar();
                        IList<Produto> produtos = daoProdutoSync.Listar();
                        IList<RecebimentoCartao> recebimentos = daoRecebimentoCartaoSync.Listar();

                        SessionSyncProvider.FechaSession(sessionSync);

                        sessionLocal = SessionSyncProvider.GetSessionLocal();

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
                    }
                    else
                    {
                        using (StreamReader streamReader = File.OpenText(Caminho))
                        {
                            string dataString = streamReader.ReadToEnd();
                            lastUpdate = DateTime.Parse(dataString);
                        }

                        AdicionaTexto($"{DataHoraAtual()}: Lendo Arquivos Remotos Com As Mudanças No Banco de Dados\n");

                        IList<EntidadeMySQL<Fornecedor>> fornecedoresRemotos = ArquivoEntidade<Fornecedor>.LerXmlRemoto(lastUpdate);
                        IList<EntidadeMySQL<Loja>> lojasRemotos = ArquivoEntidade<Loja>.LerXmlRemoto(lastUpdate);
                        IList<EntidadeMySQL<Marca>> marcasRemotos = ArquivoEntidade<Marca>.LerXmlRemoto(lastUpdate);
                        IList<EntidadeMySQL<OperadoraCartao>> operadorasRemotos = ArquivoEntidade<OperadoraCartao>.LerXmlRemoto(lastUpdate);
                        IList<EntidadeMySQL<Produto>> produtosRemotos = ArquivoEntidade<Produto>.LerXmlRemoto(lastUpdate);
                        IList<EntidadeMySQL<RecebimentoCartao>> recebimentosRemotos = ArquivoEntidade<RecebimentoCartao>.LerXmlRemoto(lastUpdate);

                        AdicionaTexto($"{DataHoraAtual()}: Atualizando Banco de Dados Local Com Arquivos Remotos\n");

                        sessionLocal = SessionSyncProvider.GetSessionLocal();

                        DAOSync<Fornecedor> daoFornecedorLocal = new DAOSync<Fornecedor>(sessionLocal);
                        DAOSync<Loja> daoLojaLocal = new DAOSync<Loja>(sessionLocal);
                        DAOSync<Marca> daoMarcaLocal = new DAOSync<Marca>(sessionLocal);
                        DAOSync<OperadoraCartao> daoOperadoraCartaoLocal = new DAOSync<OperadoraCartao>(sessionLocal);
                        DAOSync<Produto> daoProdutoLocal = new DAOSync<Produto>(sessionLocal);
                        DAOSync<RecebimentoCartao> daoRecebimentoCartaoLocal = new DAOSync<RecebimentoCartao>(sessionLocal);

                        SincronizaBancoDeDados(daoFornecedorLocal, fornecedoresRemotos);
                        SincronizaBancoDeDados(daoLojaLocal, lojasRemotos);
                        SincronizaBancoDeDados(daoMarcaLocal, marcasRemotos);
                        SincronizaBancoDeDados(daoOperadoraCartaoLocal, operadorasRemotos);
                        SincronizaBancoDeDados(daoProdutoLocal, produtosRemotos);
                        SincronizaBancoDeDados(daoRecebimentoCartaoLocal, recebimentosRemotos);

                        SessionSyncProvider.FechaSession(sessionLocal);

                        AdicionaTexto($"{DataHoraAtual()}: Atualizando Banco de Dados Remoto Com Arquivos Locais\n");

                        IList<EntidadeMySQL<Fornecedor>> fornecedoresLocais = ArquivoEntidade<Fornecedor>.LerXmlLocal();
                        IList<EntidadeMySQL<Loja>> lojasLocais = ArquivoEntidade<Loja>.LerXmlLocal();
                        IList<EntidadeMySQL<Marca>> marcasLocais = ArquivoEntidade<Marca>.LerXmlLocal();
                        IList<EntidadeMySQL<OperadoraCartao>> operadorasLocais = ArquivoEntidade<OperadoraCartao>.LerXmlLocal();
                        IList<EntidadeMySQL<Produto>> produtosLocais = ArquivoEntidade<Produto>.LerXmlLocal();
                        IList<EntidadeMySQL<RecebimentoCartao>> recebimentosLocais = ArquivoEntidade<RecebimentoCartao>.LerXmlLocal();

                        sessionSync = SessionSyncProvider.GetSessionSync();

                        DAOSync<Fornecedor> daoFornecedorSync = new DAOSync<Fornecedor>(sessionSync);
                        DAOSync<Loja> daoLojaSync = new DAOSync<Loja>(sessionSync);
                        DAOSync<Marca> daoMarcaSync = new DAOSync<Marca>(sessionSync);
                        DAOSync<OperadoraCartao> daoOperadoraCartaoSync = new DAOSync<OperadoraCartao>(sessionSync);
                        DAOSync<Produto> daoProdutoSync = new DAOSync<Produto>(sessionSync);
                        DAOSync<RecebimentoCartao> daoRecebimentoCartaoSync = new DAOSync<RecebimentoCartao>(sessionSync);

                        SincronizaBancoDeDados(daoFornecedorSync, fornecedoresLocais);
                        SincronizaBancoDeDados(daoLojaSync, lojasLocais);
                        SincronizaBancoDeDados(daoMarcaSync, marcasLocais);
                        SincronizaBancoDeDados(daoOperadoraCartaoSync, operadorasLocais);
                        SincronizaBancoDeDados(daoProdutoSync, produtosLocais);
                        SincronizaBancoDeDados(daoRecebimentoCartaoSync, recebimentosLocais);

                        SessionSyncProvider.FechaSession(sessionSync);

                        ArquivoEntidade<Fornecedor>.EnviaXmlRemoto();
                        ArquivoEntidade<Loja>.EnviaXmlRemoto();
                        ArquivoEntidade<Marca>.EnviaXmlRemoto();
                        ArquivoEntidade<OperadoraCartao>.EnviaXmlRemoto();
                        ArquivoEntidade<Produto>.EnviaXmlRemoto();
                        ArquivoEntidade<RecebimentoCartao>.EnviaXmlRemoto();

                        EsvaziaDiretorios();
                    }

                    using (StreamWriter streamWriter = new StreamWriter(Caminho, false))
                    {
                        streamWriter.WriteLine(inicioSincronizacao.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
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
        private static void SincronizaBancoDeDados<E>(DAOSync<E> dao, IList<EntidadeMySQL<E>> entidadesRemotas) where E : class
        {
            if (entidadesRemotas?.Count > 0)
            {
                IList<E> listaInsertUpdate = entidadesRemotas.Where(
                    w => w.OperacaoMySql.Equals("INSERT") || w.OperacaoMySql.Equals("UPDATE")).
                    Select(sm => sm.EntidadeSalva).ToList();

                IList<E> listaDelete = entidadesRemotas.Where(
                    w => w.OperacaoMySql.Equals("DELETE")).
                    Select(sm => sm.EntidadeSalva).ToList();

                if (listaInsertUpdate.Count > 0)
                    dao.InserirOuAtualizar(listaInsertUpdate);

                if (listaDelete.Count > 0)
                    dao.Deletar(listaDelete);
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
