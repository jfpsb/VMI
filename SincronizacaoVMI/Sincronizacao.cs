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
using SincronizacaoVMI.Model.Pix;

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
            ISession local = null, remote = null;
            try
            {
                await _semaphoreSlim.WaitAsync();
                Console.WriteLine($"Iniciando sincronização de LojaMatriz em {DateTime.Now}");
                local = SessionProvider.GetSession();
                remote = SessionProviderBackup.GetSession();
                SincronizarLojaMatriz aSync = new SincronizarLojaMatriz(local, remote);
                await aSync.Sincronizar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _semaphoreSlim.Release();
                _threadsStatus["LojaMatriz"] = false;
                SessionProvider.FechaSession(local);
                SessionProviderBackup.FechaSession(remote);
            }
        }
        private async void ElapsedtimerLojaFilial()
        {
            ISession local = null, remote = null;
            try
            {
                await _semaphoreSlim.WaitAsync();
                Console.WriteLine($"Iniciando sincronização de LojaFilial em {DateTime.Now}");
                local = SessionProvider.GetSession();
                remote = SessionProviderBackup.GetSession();
                SincronizarLojaFilial aSync = new SincronizarLojaFilial(local, remote);
                await aSync.Sincronizar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _semaphoreSlim.Release();
                _threadsStatus["LojaFilial"] = false;
                SessionProvider.FechaSession(local);
                SessionProviderBackup.FechaSession(remote);
            }
        }
        private async void ElapsedGenerico<E>() where E : AModel, new()
        {
            ISession local = null, remote = null;
            try
            {
                await _semaphoreSlim.WaitAsync();
                Console.WriteLine($"Iniciando sincronização de {typeof(E).Name} em {DateTime.Now}");
                local = SessionProvider.GetSession();
                remote = SessionProviderBackup.GetSession();
                ASincronizar<E> aSync = new SincronizarGenerico<E>(local, remote);
                await aSync.Sincronizar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _semaphoreSlim.Release();
                _threadsStatus[typeof(E).FullName] = false;
                SessionProvider.FechaSession(local);
                SessionProviderBackup.FechaSession(remote);
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
                    _threadsByType.Add(typeof(Banco).FullName, new Thread((instancia) => { ElapsedGenerico<Banco>(); }));
                    _threadsByType.Add(typeof(Adiantamento).FullName, new Thread((instancia) => { ElapsedGenerico<Adiantamento>(); }));
                    _threadsByType.Add(typeof(TipoDespesa).FullName, new Thread((instancia) => { ElapsedGenerico<TipoDespesa>(); }));
                    _threadsByType.Add("LojaMatriz", new Thread((instancia) => { ElapsedtimerLojaMatriz(); }));
                    _threadsByType.Add("LojaFilial", new Thread((instancia) => { ElapsedtimerLojaFilial(); }));
                    _threadsByType.Add(typeof(Funcionario).FullName, new Thread((instancia) => { ElapsedGenerico<Funcionario>(); }));
                    _threadsByType.Add(typeof(ContaBancaria).FullName, new Thread((instancia) => { ElapsedGenerico<ContaBancaria>(); }));
                    _threadsByType.Add(typeof(MembroFamiliar).FullName, new Thread((instancia) => { ElapsedGenerico<MembroFamiliar>(); }));
                    _threadsByType.Add(typeof(TipoContagem).FullName, new Thread((instancia) => { ElapsedGenerico<TipoContagem>(); }));
                    _threadsByType.Add(typeof(Contagem).FullName, new Thread((instancia) => { ElapsedGenerico<Contagem>(); }));
                    _threadsByType.Add(typeof(ContagemProduto).FullName, new Thread((instancia) => { ElapsedGenerico<ContagemProduto>(); }));
                    _threadsByType.Add(typeof(AliquotasImposto).FullName, new Thread((instancia) => { ElapsedGenerico<AliquotasImposto>(); }));
                    _threadsByType.Add(typeof(Representante).FullName, new Thread((instancia) => { ElapsedGenerico<Representante>(); }));
                    _threadsByType.Add(typeof(Fornecedor).FullName, new Thread((instancia) => { ElapsedGenerico<Fornecedor>(); }));
                    _threadsByType.Add(typeof(CompraDeFornecedor).FullName, new Thread((instancia) => { ElapsedGenerico<CompraDeFornecedor>(); }));
                    _threadsByType.Add(typeof(ArquivosCompraFornecedor).FullName, new Thread((instancia) => { ElapsedGenerico<ArquivosCompraFornecedor>(); }));
                    _threadsByType.Add(typeof(Bonus).FullName, new Thread((instancia) => { ElapsedGenerico<Bonus>(); }));
                    _threadsByType.Add(typeof(BonusMensal).FullName, new Thread((instancia) => { ElapsedGenerico<BonusMensal>(); }));
                    _threadsByType.Add(typeof(ChavePix).FullName, new Thread((instancia) => { ElapsedGenerico<ChavePix>(); }));
                    _threadsByType.Add(typeof(Despesa).FullName, new Thread((instancia) => { ElapsedGenerico<Despesa>(); }));
                    _threadsByType.Add(typeof(EntradaDeMercadoria).FullName, new Thread((instancia) => { ElapsedGenerico<EntradaDeMercadoria>(); }));
                    _threadsByType.Add(typeof(EntradaMercadoriaProdutoGrade).FullName, new Thread((instancia) => { ElapsedGenerico<EntradaMercadoriaProdutoGrade>(); }));
                    _threadsByType.Add(typeof(Estoque).FullName, new Thread(new ParameterizedThreadStart((instancia) => { ElapsedGenerico<Estoque>(); })));
                    _threadsByType.Add(typeof(Faltas).FullName, new Thread((instancia) => { ElapsedGenerico<Faltas>(); }));
                    _threadsByType.Add(typeof(FolhaPagamento).FullName, new Thread((instancia) => { ElapsedGenerico<FolhaPagamento>(); }));
                    _threadsByType.Add(typeof(TipoGrade).FullName, new Thread((instancia) => { ElapsedGenerico<TipoGrade>(); }));
                    _threadsByType.Add(typeof(Grade).FullName, new Thread((instancia) => { ElapsedGenerico<Grade>(); }));
                    _threadsByType.Add(typeof(HistoricoProdutoGrade).FullName, new Thread((instancia) => { ElapsedGenerico<HistoricoProdutoGrade>(); }));
                    _threadsByType.Add(typeof(TipoHoraExtra).FullName, new Thread((instancia) => { ElapsedGenerico<TipoHoraExtra>(); }));
                    _threadsByType.Add(typeof(HoraExtra).FullName, new Thread((instancia) => { ElapsedGenerico<HoraExtra>(); }));
                    _threadsByType.Add(typeof(InscricaoImobiliaria).FullName, new Thread((instancia) => { ElapsedGenerico<InscricaoImobiliaria>(); }));
                    _threadsByType.Add(typeof(Marca).FullName, new Thread((instancia) => { ElapsedGenerico<Marca>(); }));
                    _threadsByType.Add(typeof(OperadoraCartao).FullName, new Thread((instancia) => { ElapsedGenerico<OperadoraCartao>(); }));
                    _threadsByType.Add(typeof(Parcela).FullName, new Thread((instancia) => { ElapsedGenerico<Parcela>(); }));
                    _threadsByType.Add(typeof(Produto).FullName, new Thread((instancia) => { ElapsedGenerico<Produto>(); }));
                    _threadsByType.Add(typeof(ProdutoGrade).FullName, new Thread((instancia) => { ElapsedGenerico<ProdutoGrade>(); }));
                    _threadsByType.Add(typeof(RecebimentoCartao).FullName, new Thread((instancia) => { ElapsedGenerico<RecebimentoCartao>(); }));
                    _threadsByType.Add(typeof(SubGrade).FullName, new Thread((instancia) => { ElapsedGenerico<SubGrade>(); }));
                    _threadsByType.Add(typeof(OperadoraCartaoId).FullName, new Thread((instancia) => { ElapsedGenerico<OperadoraCartaoId>(); }));
                    _threadsByType.Add(typeof(Ferias).FullName, new Thread((instancia) => { ElapsedGenerico<Ferias>(); }));
                    _threadsByType.Add(typeof(Calendario).FullName, new Thread((instancia) => { ElapsedGenerico<Calendario>(); }));
                    _threadsByType.Add(typeof(Cobranca).FullName, new Thread((instancia) => { ElapsedGenerico<Cobranca>(); }));
                    _threadsByType.Add(typeof(Devolucao).FullName, new Thread((instancia) => { ElapsedGenerico<Devolucao>(); }));
                    _threadsByType.Add(typeof(Horario).FullName, new Thread((instancia) => { ElapsedGenerico<Horario>(); }));
                    _threadsByType.Add(typeof(Loc).FullName, new Thread((instancia) => { ElapsedGenerico<Loc>(); }));
                    _threadsByType.Add(typeof(Pagador).FullName, new Thread((instancia) => { ElapsedGenerico<Pagador>(); }));
                    _threadsByType.Add(typeof(Pix).FullName, new Thread((instancia) => { ElapsedGenerico<Pix>(); }));
                    _threadsByType.Add(typeof(QRCode).FullName, new Thread((instancia) => { ElapsedGenerico<QRCode>(); }));
                    _threadsByType.Add(typeof(Valor).FullName, new Thread((instancia) => { ElapsedGenerico<Valor>(); }));
                    _threadsByType.Add(typeof(PontoEletronico).FullName, new Thread((instancia) => { ElapsedGenerico<PontoEletronico>(); }));
                    _threadsByType.Add(typeof(IntervaloPonto).FullName, new Thread((instancia) => { ElapsedGenerico<IntervaloPonto>(); }));
                    _threadsByType.Add(typeof(VendaEmCartao).FullName, new Thread((instancia) => { ElapsedGenerico<VendaEmCartao>(); }));
                    _threadsByType.Add(typeof(ParcelaCartao).FullName, new Thread((instancia) => { ElapsedGenerico<ParcelaCartao>(); }));

                    foreach (var t in _threadsByType)
                    {
                        _threadsStatus.Add(t.Key, false);
                    }
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
                                var tipo = Type.GetType($"{item.Key}, SincronizacaoVMI", true);
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
