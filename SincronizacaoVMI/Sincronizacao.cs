using MySql.Data.MySqlClient;
using NHibernate;
using SincronizacaoServico.Util;
using SincronizacaoVMI.Banco;
using SincronizacaoVMI.Model;
using SincronizacaoVMI.Util;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace SincronizacaoServico
{
    public class Sincronizacao
    {
        private Timer timer;

        public Sincronizacao()
        {
            timer = new System.Timers.Timer(2000) { AutoReset = false };
            timer.Elapsed += ExecutaSync;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            SessionProvider.FechaSessionFactory();
            SessionProviderBackup.FechaSessionFactory();
        }

        private async void ExecutaSync(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                if (SessionProvider.SessionFactory == null)
                    SessionProvider.SessionFactory = SessionProvider.BuildSessionFactory();
                if (SessionProviderBackup.BackupSessionFactory == null)
                    SessionProviderBackup.BackupSessionFactory = SessionProviderBackup.BuildSessionFactory();
                Console.WriteLine($"Iniciando sincronização em {e.SignalTime}");
                await StartSync();
                Console.WriteLine($"Fim de sincronização em {DateTime.Now}");
            }
            catch (MySqlException mex)
            {
                Console.WriteLine("Erro ao abrir conexão com banco de dados local ou remoto." +
                    "\nSem esta conexão a sincronização não é possível.\n" + mex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                timer.Start();
            }
        }

        private async Task StartSync()
        {
            ISession local = SessionProvider.GetSession();
            ISession remote = SessionProviderBackup.GetSession();

            try
            {
                ASincronizar<Banco> aBanco = new SincronizarGenerico<Banco>(local, remote);
                ASincronizar<ContaBancaria> aContaBancaria = new SincronizarGenerico<ContaBancaria>(local, remote);
                ASincronizar<Adiantamento> aAdiantamento = new SincronizarGenerico<Adiantamento>(local, remote);
                ASincronizar<TipoDespesa> aTipoDespesa = new SincronizarGenerico<TipoDespesa>(local, remote);
                ASincronizar<Loja> aLoja = new SincronizarGenerico<Loja>(local, remote);
                ASincronizar<Funcionario> aFuncionario = new SincronizarGenerico<Funcionario>(local, remote);
                ASincronizar<MembroFamiliar> aMembroFamiliar = new SincronizarGenerico<MembroFamiliar>(local, remote);
                ASincronizar<TipoContagem> aTipoContagem = new SincronizarGenerico<TipoContagem>(local, remote);
                ASincronizar<Contagem> aContagem = new SincronizarGenerico<Contagem>(local, remote);
                ASincronizar<ContagemProduto> aContagemProduto = new SincronizarGenerico<ContagemProduto>(local, remote);
                ASincronizar<AliquotasImposto> aAliquotasImposto = new SincronizarGenerico<AliquotasImposto>(local, remote);
                ASincronizar<Representante> aRepresentante = new SincronizarGenerico<Representante>(local, remote);
                ASincronizar<Fornecedor> aFornecedor = new SincronizarGenerico<Fornecedor>(local, remote);
                ASincronizar<CompraDeFornecedor> aCompraDeFornecedor = new SincronizarGenerico<CompraDeFornecedor>(local, remote);
                ASincronizar<ArquivosCompraFornecedor> aArquivosCompraFornecedor = new SincronizarGenerico<ArquivosCompraFornecedor>(local, remote);
                ASincronizar<Bonus> aBonus = new SincronizarGenerico<Bonus>(local, remote);
                ASincronizar<BonusMensal> aBonusMensal = new SincronizarGenerico<BonusMensal>(local, remote);
                ASincronizar<ChavePix> aChavePix = new SincronizarGenerico<ChavePix>(local, remote);
                ASincronizar<Despesa> aDespesa = new SincronizarGenerico<Despesa>(local, remote);
                ASincronizar<EntradaDeMercadoria> aEntradaDeMercadoria = new SincronizarGenerico<EntradaDeMercadoria>(local, remote);
                ASincronizar<EntradaMercadoriaProdutoGrade> aEntradaMercadoriaProdutoGrade = new SincronizarGenerico<EntradaMercadoriaProdutoGrade>(local, remote);
                ASincronizar<Estoque> aEstoque = new SincronizarGenerico<Estoque>(local, remote);
                ASincronizar<Faltas> aFaltas = new SincronizarGenerico<Faltas>(local, remote);
                ASincronizar<FolhaPagamento> aFolhaPagamento = new SincronizarGenerico<FolhaPagamento>(local, remote);
                ASincronizar<TipoGrade> aTipoGrade = new SincronizarGenerico<TipoGrade>(local, remote);
                ASincronizar<Grade> aGrade = new SincronizarGenerico<Grade>(local, remote);
                ASincronizar<HistoricoProdutoGrade> aHistoricoProdutoGrade = new SincronizarGenerico<HistoricoProdutoGrade>(local, remote);
                ASincronizar<TipoHoraExtra> aTipoHoraExtra = new SincronizarGenerico<TipoHoraExtra>(local, remote);
                ASincronizar<HoraExtra> aHoraExtra = new SincronizarGenerico<HoraExtra>(local, remote);
                ASincronizar<InscricaoImobiliaria> aInscricaoImobiliaria = new SincronizarGenerico<InscricaoImobiliaria>(local, remote);
                ASincronizar<Marca> aMarca = new SincronizarGenerico<Marca>(local, remote);
                ASincronizar<OperadoraCartao> aOperadoraCartao = new SincronizarGenerico<OperadoraCartao>(local, remote);
                ASincronizar<Parcela> aParcela = new SincronizarGenerico<Parcela>(local, remote);
                ASincronizar<Produto> aProduto = new SincronizarGenerico<Produto>(local, remote);
                ASincronizar<ProdutoGrade> aProdutoGrade = new SincronizarGenerico<ProdutoGrade>(local, remote);
                ASincronizar<RecebimentoCartao> aRecebimentoCartao = new SincronizarGenerico<RecebimentoCartao>(local, remote);
                ASincronizar<SubGrade> aSubGrade = new SincronizarGenerico<SubGrade>(local, remote);

                await aBanco.Sincronizar();
                await aContaBancaria.Sincronizar();
                await aAdiantamento.Sincronizar();
                await aTipoDespesa.Sincronizar();
                await aLoja.Sincronizar();
                await aFuncionario.Sincronizar();
                await aMembroFamiliar.Sincronizar();
                await aTipoContagem.Sincronizar();
                await aContagem.Sincronizar();
                await aContagemProduto.Sincronizar();
                await aAliquotasImposto.Sincronizar();
                await aRepresentante.Sincronizar();
                await aFornecedor.Sincronizar();
                await aCompraDeFornecedor.Sincronizar();
                await aArquivosCompraFornecedor.Sincronizar();
                await aBonus.Sincronizar();
                await aBonusMensal.Sincronizar();
                await aChavePix.Sincronizar();
                await aDespesa.Sincronizar();
                await aEntradaDeMercadoria.Sincronizar();
                await aEntradaMercadoriaProdutoGrade.Sincronizar();
                await aEstoque.Sincronizar();
                await aFaltas.Sincronizar();
                await aFolhaPagamento.Sincronizar();
                await aTipoGrade.Sincronizar();
                await aGrade.Sincronizar();
                await aHistoricoProdutoGrade.Sincronizar();
                await aTipoHoraExtra.Sincronizar();
                await aHoraExtra.Sincronizar();
                await aInscricaoImobiliaria.Sincronizar();
                await aMarca.Sincronizar();
                await aOperadoraCartao.Sincronizar();
                await aParcela.Sincronizar();
                await aProduto.Sincronizar();
                await aProdutoGrade.Sincronizar();
                await aRecebimentoCartao.Sincronizar();
                await aSubGrade.Sincronizar();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                SessionProvider.FechaSession(local);
                SessionProviderBackup.FechaSession(remote);
            }
        }
    }
}
