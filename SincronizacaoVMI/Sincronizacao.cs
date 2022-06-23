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
        private Dictionary<string, Thread> _threadsByType;
        private Dictionary<string, bool> _threadsStatus;
        private SemaphoreSlim _semaphoreSlim;

        public Sincronizacao()
        {
            _threadsByType = new Dictionary<string, Thread>();
            _threadsStatus = new Dictionary<string, bool>();
            _semaphoreSlim = new SemaphoreSlim(3);
            timerConexao = new Timer(5000) { AutoReset = false };
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
                _threadsStatus["LojaMatriz"] = false;
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
                _threadsStatus["LojaFilial"] = false;
            }
        }
        private async void ElapsedGenerico<E>() where E : AModel, new()
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
                _threadsStatus[typeof(E).Name] = false;
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

                if (_threadsByType.Count == 0)
                {
                    _threadsByType.Add("Banco", new Thread((instancia) => { ElapsedGenerico<Banco>(); }));
                    _threadsByType.Add("Adiantamento", new Thread((instancia) => { ElapsedGenerico<Adiantamento>(); }));
                    _threadsByType.Add("TipoDespesa", new Thread((instancia) => { ElapsedGenerico<TipoDespesa>(); }));
                    _threadsByType.Add("LojaMatriz", new Thread((instancia) => { ElapsedtimerLojaMatriz(); }));
                    _threadsByType.Add("LojaFilial", new Thread((instancia) => { ElapsedtimerLojaFilial(); }));
                    _threadsByType.Add("Funcionario", new Thread((instancia) => { ElapsedGenerico<Funcionario>(); }));
                    _threadsByType.Add("ContaBancaria", new Thread((instancia) => { ElapsedGenerico<ContaBancaria>(); }));
                    _threadsByType.Add("MembroFamiliar", new Thread((instancia) => { ElapsedGenerico<MembroFamiliar>(); }));
                    _threadsByType.Add("TipoContagem", new Thread((instancia) => { ElapsedGenerico<TipoContagem>(); }));
                    _threadsByType.Add("Contagem", new Thread((instancia) => { ElapsedGenerico<Contagem>(); }));
                    _threadsByType.Add("ContagemProduto", new Thread((instancia) => { ElapsedGenerico<ContagemProduto>(); }));
                    _threadsByType.Add("AliquotasImposto", new Thread((instancia) => { ElapsedGenerico<AliquotasImposto>(); }));
                    _threadsByType.Add("Representante", new Thread((instancia) => { ElapsedGenerico<Representante>(); }));
                    _threadsByType.Add("Fornecedor", new Thread((instancia) => { ElapsedGenerico<Fornecedor>(); }));
                    _threadsByType.Add("CompraDeFornecedor", new Thread((instancia) => { ElapsedGenerico<CompraDeFornecedor>(); }));
                    _threadsByType.Add("ArquivosCompraFornecedor", new Thread((instancia) => { ElapsedGenerico<ArquivosCompraFornecedor>(); }));
                    _threadsByType.Add("Bonus", new Thread((instancia) => { ElapsedGenerico<Bonus>(); }));
                    _threadsByType.Add("BonusMensal", new Thread((instancia) => { ElapsedGenerico<BonusMensal>(); }));
                    _threadsByType.Add("ChavePix", new Thread((instancia) => { ElapsedGenerico<ChavePix>(); }));
                    _threadsByType.Add("Despesa", new Thread((instancia) => { ElapsedGenerico<Despesa>(); }));
                    _threadsByType.Add("EntradaDeMercadoria", new Thread((instancia) => { ElapsedGenerico<EntradaDeMercadoria>(); }));
                    _threadsByType.Add("EntradaMercadoriaProdutoGrade", new Thread((instancia) => { ElapsedGenerico<EntradaMercadoriaProdutoGrade>(); }));
                    _threadsByType.Add("Estoque", new Thread(new ParameterizedThreadStart((instancia) => { ElapsedGenerico<Estoque>(); })));
                    _threadsByType.Add("Faltas", new Thread((instancia) => { ElapsedGenerico<Faltas>(); }));
                    _threadsByType.Add("FolhaPagamento", new Thread((instancia) => { ElapsedGenerico<FolhaPagamento>(); }));
                    _threadsByType.Add("TipoGrade", new Thread((instancia) => { ElapsedGenerico<TipoGrade>(); }));
                    _threadsByType.Add("Grade", new Thread((instancia) => { ElapsedGenerico<Grade>(); }));
                    _threadsByType.Add("HistoricoProdutoGrade", new Thread((instancia) => { ElapsedGenerico<HistoricoProdutoGrade>(); }));
                    _threadsByType.Add("TipoHoraExtra", new Thread((instancia) => { ElapsedGenerico<TipoHoraExtra>(); }));
                    _threadsByType.Add("HoraExtra", new Thread((instancia) => { ElapsedGenerico<HoraExtra>(); }));
                    _threadsByType.Add("InscricaoImobiliaria", new Thread((instancia) => { ElapsedGenerico<InscricaoImobiliaria>(); }));
                    _threadsByType.Add("Marca", new Thread((instancia) => { ElapsedGenerico<Marca>(); }));
                    _threadsByType.Add("OperadoraCartao", new Thread((instancia) => { ElapsedGenerico<OperadoraCartao>(); }));
                    _threadsByType.Add("Parcela", new Thread((instancia) => { ElapsedGenerico<Parcela>(); }));
                    _threadsByType.Add("Produto", new Thread((instancia) => { ElapsedGenerico<Produto>(); }));
                    _threadsByType.Add("ProdutoGrade", new Thread((instancia) => { ElapsedGenerico<ProdutoGrade>(); }));
                    _threadsByType.Add("RecebimentoCartao", new Thread((instancia) => { ElapsedGenerico<RecebimentoCartao>(); }));
                    _threadsByType.Add("SubGrade", new Thread((instancia) => { ElapsedGenerico<SubGrade>(); }));
                    _threadsByType.Add("OperadoraCartaoId", new Thread((instancia) => { ElapsedGenerico<OperadoraCartaoId>(); }));

                    _threadsStatus.Add("Banco", false);
                    _threadsStatus.Add("Adiantamento", false);
                    _threadsStatus.Add("TipoDespesa", false);
                    _threadsStatus.Add("LojaMatriz", false);
                    _threadsStatus.Add("LojaFilial", false);
                    _threadsStatus.Add("Funcionario", false);
                    _threadsStatus.Add("ContaBancaria", false);
                    _threadsStatus.Add("MembroFamiliar", false);
                    _threadsStatus.Add("TipoContagem", false);
                    _threadsStatus.Add("Contagem", false);
                    _threadsStatus.Add("ContagemProduto", false);
                    _threadsStatus.Add("AliquotasImposto", false);
                    _threadsStatus.Add("Representante", false);
                    _threadsStatus.Add("Fornecedor", false);
                    _threadsStatus.Add("CompraDeFornecedor", false);
                    _threadsStatus.Add("ArquivosCompraFornecedor", false);
                    _threadsStatus.Add("Bonus", false);
                    _threadsStatus.Add("BonusMensal", false);
                    _threadsStatus.Add("ChavePix", false);
                    _threadsStatus.Add("Despesa", false);
                    _threadsStatus.Add("EntradaDeMercadoria", false);
                    _threadsStatus.Add("EntradaMercadoriaProdutoGrade", false);
                    _threadsStatus.Add("Estoque", false);
                    _threadsStatus.Add("Faltas", false);
                    _threadsStatus.Add("FolhaPagamento", false);
                    _threadsStatus.Add("TipoGrade", false);
                    _threadsStatus.Add("Grade", false);
                    _threadsStatus.Add("HistoricoProdutoGrade", false);
                    _threadsStatus.Add("TipoHoraExtra", false);
                    _threadsStatus.Add("HoraExtra", false);
                    _threadsStatus.Add("InscricaoImobiliaria", false);
                    _threadsStatus.Add("Marca", false);
                    _threadsStatus.Add("OperadoraCartao", false);
                    _threadsStatus.Add("Parcela", false);
                    _threadsStatus.Add("Produto", false);
                    _threadsStatus.Add("ProdutoGrade", false);
                    _threadsStatus.Add("RecebimentoCartao", false);
                    _threadsStatus.Add("SubGrade", false);
                    _threadsStatus.Add("OperadoraCartaoId", false);
                }
                else
                {
                    foreach (var item in _threadsByType)
                    {
                        if (_threadsStatus[item.Key]) continue;

                        if (item.Key.Equals("LojaMatriz"))
                        {
                            _threadsByType[item.Key] = new Thread((instancia) => { ElapsedtimerLojaMatriz(); });
                        }
                        else if (item.Key.Equals("LojaFilial"))
                        {
                            _threadsByType[item.Key] = new Thread((instancia) => { ElapsedtimerLojaFilial(); });
                        }
                        else
                        {
                            _threadsByType[item.Key] = new Thread((instancia) =>
                            {
                                var tipo = Type.GetType($"SincronizacaoVMI.Model.{item.Key}, SincronizacaoVMI", true);
                                var metodo = instancia.GetType().GetMethod("ElapsedGenerico", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                                var generMetodo = metodo.MakeGenericMethod(tipo);
                                generMetodo.Invoke(instancia, null);
                            });
                        }
                    }
                }

                foreach (var t in _threadsByType)
                {
                    if (_threadsStatus[t.Key]) continue;
                    t.Value.Start(this);
                    _threadsStatus[t.Key] = true;
                }
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("Erro ao abrir conexão com banco de dados local ou remoto." +
                    "\nSem esta conexão a sincronização não é possível.\nNova tentativa em 5 segundos." + mex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Nova tentativa de criar SessionFactory em 5 segundos.");
            }
            finally
            {
                timerConexao.Start();
            }
        }
    }
}
