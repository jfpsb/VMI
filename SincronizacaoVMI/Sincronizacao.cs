using MySql.Data.MySqlClient;
using NHibernate;
using SincronizacaoVMI.Util;
using SincronizacaoVMI.BancoDeDados;
using SincronizacaoVMI.Model;
using System;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Collections.Generic;

namespace SincronizacaoVMI
{
    public class Sincronizacao
    {
        private Timer timerConexao;
        private Thread threadBanco;
        private Thread threadAdiantamento;
        private Thread threadTipoDespesa;
        private Thread threadLojaMatriz;
        private Thread threadLojaFilial;
        private Thread threadFuncionario;
        private Thread threadContaBancaria;
        private Thread threadMembroFamiliar;
        private Thread threadTipoContagem;
        private Thread threadContagem;
        private Thread threadContagemProduto;
        private Thread threadAliquotasImposto;
        private Thread threadRepresentante;
        private Thread threadFornecedor;
        private Thread threadCompraDeFornecedor;
        private Thread threadArquivosCompraFornecedor;
        private Thread threadBonus;
        private Thread threadBonusMensal;
        private Thread threadChavePix;
        private Thread threadDespesa;
        private Thread threadEntradaDeMercadoria;
        private Thread threadEntradaDeMercadoriaProdutoGrade;
        private Thread threadEstoque;
        private Thread threadFaltas;
        private Thread threadFolhaPagamento;
        private Thread threadTipoGrade;
        private Thread threadGrade;
        private Thread threadHistoricoProdutoGrade;
        private Thread threadTipoHoraExtra;
        private Thread threadHoraExtra;
        private Thread threadInscricaoImobiliaria;
        private Thread threadMarca;
        private Thread threadOperadoraCartao;
        private Thread threadParcela;
        private Thread threadProduto;
        private Thread threadProdutoGrade;
        private Thread threadRecebimentoCartao;
        private Thread threadSubGrade;

        private IList<Thread> _threads;
        private SemaphoreSlim _semaphoreSlim;

        public Sincronizacao()
        {
            _threads = new List<Thread>();
            _semaphoreSlim = new SemaphoreSlim(3);
            timerConexao = new Timer(3000) { AutoReset = false };
            timerConexao.Elapsed += CriarSessionFactory;
        }

        private async void ElapsedtimerLojaMatriz()
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                Console.WriteLine($"Iniciando sincronização de LojaMatriz em {DateTime.Now}");
                ISession local = SessionProvider.GetSession();
                ISession remote = SessionProviderBackup.GetSession();
                SincronizarLojaMatriz aSync = new SincronizarLojaMatriz(local, remote);
                await aSync.Sincronizar();
                SessionProvider.FechaSession(local);
                SessionProviderBackup.FechaSession(remote);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        private async void ElapsedtimerLojaFilial()
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                Console.WriteLine($"Iniciando sincronização de LojaFilial em {DateTime.Now}");
                ISession local = SessionProvider.GetSession();
                ISession remote = SessionProviderBackup.GetSession();
                SincronizarLojaFilial aSync = new SincronizarLojaFilial(local, remote);
                await aSync.Sincronizar();
                SessionProvider.FechaSession(local);
                SessionProviderBackup.FechaSession(remote);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        private async void ElapsedGenerico<E>() where E : AModel, IModel, new()
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                Console.WriteLine($"Iniciando sincronização de {typeof(E).Name} em {DateTime.Now}");
                ISession local = SessionProvider.GetSession();
                ISession remote = SessionProviderBackup.GetSession();
                ASincronizar<E> aSync = new SincronizarGenerico<E>(local, remote);
                await aSync.Sincronizar();
                SessionProvider.FechaSession(local);
                SessionProviderBackup.FechaSession(remote);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        public void Start()
        {
            timerConexao.Start();
        }
        public void Stop()
        {
            timerConexao.Stop();
            timerConexao.Dispose();
            SessionProvider.FechaSessionFactory();
            SessionProviderBackup.FechaSessionFactory();
        }
        private void CriarSessionFactory(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerConexao.Stop();

                if (SessionProvider.SessionFactory == null)
                    SessionProvider.SessionFactory = SessionProvider.BuildSessionFactory();
                if (SessionProviderBackup.BackupSessionFactory == null)
                    SessionProviderBackup.BackupSessionFactory = SessionProviderBackup.BuildSessionFactory();

                threadBanco = new Thread(() => { ElapsedGenerico<Banco>(); });
                threadAdiantamento = new Thread(() => { ElapsedGenerico<Adiantamento>(); });
                threadTipoDespesa = new Thread(() => { ElapsedGenerico<TipoDespesa>(); });
                threadLojaMatriz = new Thread(() => { ElapsedtimerLojaMatriz(); });
                threadLojaFilial = new Thread(() => { ElapsedtimerLojaFilial(); });
                threadFuncionario = new Thread(() => { ElapsedGenerico<Funcionario>(); });
                threadContaBancaria = new Thread(() => { ElapsedGenerico<ContaBancaria>(); });
                threadMembroFamiliar = new Thread(() => { ElapsedGenerico<MembroFamiliar>(); });
                threadTipoContagem = new Thread(() => { ElapsedGenerico<TipoContagem>(); });
                threadContagem = new Thread(() => { ElapsedGenerico<Contagem>(); });
                threadContagemProduto = new Thread(() => { ElapsedGenerico<ContagemProduto>(); });
                threadAliquotasImposto = new Thread(() => { ElapsedGenerico<AliquotasImposto>(); });
                threadRepresentante = new Thread(() => { ElapsedGenerico<Representante>(); });
                threadFornecedor = new Thread(() => { ElapsedGenerico<Fornecedor>(); });
                threadCompraDeFornecedor = new Thread(() => { ElapsedGenerico<CompraDeFornecedor>(); });
                threadArquivosCompraFornecedor = new Thread(() => { ElapsedGenerico<ArquivosCompraFornecedor>(); });
                threadBonus = new Thread(() => { ElapsedGenerico<Bonus>(); });
                threadBonusMensal = new Thread(() => { ElapsedGenerico<BonusMensal>(); });
                threadChavePix = new Thread(() => { ElapsedGenerico<ChavePix>(); });
                threadDespesa = new Thread(() => { ElapsedGenerico<Despesa>(); });
                threadEntradaDeMercadoria = new Thread(() => { ElapsedGenerico<EntradaDeMercadoria>(); });
                threadEntradaDeMercadoriaProdutoGrade = new Thread(() => { ElapsedGenerico<EntradaMercadoriaProdutoGrade>(); });
                threadEstoque = new Thread(() => { ElapsedGenerico<Estoque>(); });
                threadFaltas = new Thread(() => { ElapsedGenerico<Faltas>(); });
                threadFolhaPagamento = new Thread(() => { ElapsedGenerico<FolhaPagamento>(); });
                threadTipoGrade = new Thread(() => { ElapsedGenerico<TipoGrade>(); });
                threadGrade = new Thread(() => { ElapsedGenerico<Grade>(); });
                threadHistoricoProdutoGrade = new Thread(() => { ElapsedGenerico<HistoricoProdutoGrade>(); });
                threadTipoHoraExtra = new Thread(() => { ElapsedGenerico<TipoHoraExtra>(); });
                threadHoraExtra = new Thread(() => { ElapsedGenerico<HoraExtra>(); });
                threadInscricaoImobiliaria = new Thread(() => { ElapsedGenerico<InscricaoImobiliaria>(); });
                threadMarca = new Thread(() => { ElapsedGenerico<Marca>(); });
                threadOperadoraCartao = new Thread(() => { ElapsedGenerico<OperadoraCartao>(); });
                threadParcela = new Thread(() => { ElapsedGenerico<Parcela>(); });
                threadProduto = new Thread(() => { ElapsedGenerico<Produto>(); });
                threadProdutoGrade = new Thread(() => { ElapsedGenerico<ProdutoGrade>(); });
                threadRecebimentoCartao = new Thread(() => { ElapsedGenerico<RecebimentoCartao>(); });
                threadSubGrade = new Thread(() => { ElapsedGenerico<SubGrade>(); });

                _threads.Clear();
                _threads.Add(threadBanco);
                _threads.Add(threadAdiantamento);
                _threads.Add(threadTipoDespesa);
                _threads.Add(threadLojaMatriz);
                _threads.Add(threadLojaFilial);
                _threads.Add(threadFuncionario);
                _threads.Add(threadContaBancaria);
                _threads.Add(threadMembroFamiliar);
                _threads.Add(threadTipoContagem);
                _threads.Add(threadContagem);
                _threads.Add(threadContagemProduto);
                _threads.Add(threadAliquotasImposto);
                _threads.Add(threadRepresentante);
                _threads.Add(threadFornecedor);
                _threads.Add(threadCompraDeFornecedor);
                _threads.Add(threadArquivosCompraFornecedor);
                _threads.Add(threadBonus);
                _threads.Add(threadBonusMensal);
                _threads.Add(threadChavePix);
                _threads.Add(threadDespesa);
                _threads.Add(threadEntradaDeMercadoria);
                _threads.Add(threadEntradaDeMercadoriaProdutoGrade);
                _threads.Add(threadEstoque);
                _threads.Add(threadFaltas);
                _threads.Add(threadFolhaPagamento);
                _threads.Add(threadTipoGrade);
                _threads.Add(threadGrade);
                _threads.Add(threadHistoricoProdutoGrade);
                _threads.Add(threadTipoHoraExtra);
                _threads.Add(threadHoraExtra);
                _threads.Add(threadInscricaoImobiliaria);
                _threads.Add(threadMarca);
                _threads.Add(threadOperadoraCartao);
                _threads.Add(threadParcela);
                _threads.Add(threadProduto);
                _threads.Add(threadProdutoGrade);
                _threads.Add(threadRecebimentoCartao);
                _threads.Add(threadSubGrade);

                foreach (Thread t in _threads)
                {
                    t.Start();
                }
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("Erro ao abrir conexão com banco de dados local ou remoto." +
                    "\nSem esta conexão a sincronização não é possível.\nNova tentativa em 3 segundos." + mex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Nova tentativa de criar SessionFactory em 3 segundos.");
            }
            finally
            {
                timerConexao.Start();
            }
        }
    }
}
