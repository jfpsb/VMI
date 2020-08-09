using NHibernate;
using NHibernate.Stat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public class Sincronizacao
    {
        private static ISession _localSession;
        private static ISession _remoteSession;
        public async static void SincronizaDatabase()
        {
            var toSend = GlobalConfigs.DATABASELOG.Where(w => w.EnviadoAoServidor == false).ToList();

            _localSession = LocalSessionProvider.GetSession();
            _remoteSession = RemoteSessionProvider.GetSession();

            foreach (var log in toSend)
            {
                if (log.Topico.Contains("/adiantamento/"))
                {
                    var localAdiantamento = _localSession.Get<Adiantamento>(log.Chaves["Id"]);
                    var remoteAdiantamento = _remoteSession.Get<Adiantamento>(log.Chaves["Id"]);

                    if (localAdiantamento == null)
                    {
                        _localSession.Replicate(remoteAdiantamento, ReplicationMode.Overwrite);
                    }
                    else if (remoteAdiantamento == null)
                    {
                        _remoteSession.Replicate(localAdiantamento, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/bonus/"))
                {
                    var localBonus = _localSession.Get<Bonus>(log.Chaves["Id"]);
                    var remoteBonus = _remoteSession.Get<Bonus>(log.Chaves["Id"]);

                    if (localBonus == null)
                    {
                        _localSession.Replicate(remoteBonus, ReplicationMode.Overwrite);
                    }
                    else if (remoteBonus == null)
                    {
                        _remoteSession.Replicate(localBonus, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/contagem/"))
                {
                    var loja = _localSession.Get<Loja>(log.Chaves["Loja"]);
                    var contagem = new Contagem()
                    {
                        Data = DateTime.Parse(log.Chaves["Data"]),
                        Loja = loja
                    };

                    var localContagem = _localSession.Get<Bonus>(contagem);
                    var remoteContagem = _remoteSession.Get<Bonus>(contagem);

                    if (localContagem == null)
                    {
                        _localSession.Replicate(remoteContagem, ReplicationMode.Overwrite);
                    }
                    else if (remoteContagem == null)
                    {
                        _remoteSession.Replicate(localContagem, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/contagemproduto/"))
                {
                    var localContagemProduto = _localSession.Get<ContagemProduto>(log.Chaves["Id"]);
                    var remoteContagemProduto = _remoteSession.Get<ContagemProduto>(log.Chaves["Id"]);

                    if (localContagemProduto == null)
                    {
                        _localSession.Replicate(remoteContagemProduto, ReplicationMode.Overwrite);
                    }
                    else if (remoteContagemProduto == null)
                    {
                        _remoteSession.Replicate(localContagemProduto, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/folhapagamento/"))
                {
                    var localFolhaPagamento = _localSession.Get<FolhaPagamento>(log.Chaves["Id"]);
                    var remoteFolhaPagamento = _remoteSession.Get<FolhaPagamento>(log.Chaves["Id"]);

                    if (localFolhaPagamento == null)
                    {
                        _localSession.Replicate(remoteFolhaPagamento, ReplicationMode.Overwrite);
                    }
                    else if (remoteFolhaPagamento == null)
                    {
                        _remoteSession.Replicate(localFolhaPagamento, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/fornecedor/"))
                {
                    var localFornecedor = _localSession.Get<FolhaPagamento>(log.Chaves["Cnpj"]);
                    var remoteFornecedor = _remoteSession.Get<FolhaPagamento>(log.Chaves["Cnpj"]);

                    if (localFornecedor == null)
                    {
                        _localSession.Replicate(remoteFornecedor, ReplicationMode.Overwrite);
                    }
                    else if (remoteFornecedor == null)
                    {
                        _remoteSession.Replicate(localFornecedor, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/funcionario/"))
                {
                    var localFuncionario = _localSession.Get<FolhaPagamento>(log.Chaves["Cpf"]);
                    var remoteFuncionario = _remoteSession.Get<FolhaPagamento>(log.Chaves["Cpf"]);

                    if (localFuncionario == null)
                    {
                        _localSession.Replicate(remoteFuncionario, ReplicationMode.Overwrite);
                    }
                    else if (remoteFuncionario == null)
                    {
                        _remoteSession.Replicate(localFuncionario, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/loja/"))
                {
                    var localLoja = _localSession.Get<FolhaPagamento>(log.Chaves["Cnpj"]);
                    var remoteLoja = _remoteSession.Get<FolhaPagamento>(log.Chaves["Cnpj"]);

                    if (localLoja == null)
                    {
                        _localSession.Replicate(remoteLoja, ReplicationMode.Overwrite);
                    }
                    else if (remoteLoja == null)
                    {
                        _remoteSession.Replicate(localLoja, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/marca/"))
                {
                    var localMarca = _localSession.Get<FolhaPagamento>(log.Chaves["Nome"]);
                    var remoteMarca = _remoteSession.Get<FolhaPagamento>(log.Chaves["Nome"]);

                    if (localMarca == null)
                    {
                        _localSession.Replicate(remoteMarca, ReplicationMode.Overwrite);
                    }
                    else if (remoteMarca == null)
                    {
                        _remoteSession.Replicate(localMarca, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/metaloja/"))
                {
                    var localMetaLoja = _localSession.Get<FolhaPagamento>(log.Chaves["Id"]);
                    var remoteMetaLoja = _remoteSession.Get<FolhaPagamento>(log.Chaves["Id"]);

                    if (localMetaLoja == null)
                    {
                        _localSession.Replicate(remoteMetaLoja, ReplicationMode.Overwrite);
                    }
                    else if (remoteMetaLoja == null)
                    {
                        _remoteSession.Replicate(localMetaLoja, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/operadoracartao/"))
                {
                    var localOperadoraCartao = _localSession.Get<FolhaPagamento>(log.Chaves["Nome"]);
                    var remoteOperadoraCartao = _remoteSession.Get<FolhaPagamento>(log.Chaves["Nome"]);

                    if (localOperadoraCartao == null)
                    {
                        _localSession.Replicate(remoteOperadoraCartao, ReplicationMode.Overwrite);
                    }
                    else if (remoteOperadoraCartao == null)
                    {
                        _remoteSession.Replicate(localOperadoraCartao, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/parcela/"))
                {
                    var localParcela = _localSession.Get<FolhaPagamento>(log.Chaves["Id"]);
                    var remoteParcela = _remoteSession.Get<FolhaPagamento>(log.Chaves["Id"]);

                    if (localParcela == null)
                    {
                        _localSession.Replicate(remoteParcela, ReplicationMode.Overwrite);
                    }
                    else if (remoteParcela == null)
                    {
                        _remoteSession.Replicate(localParcela, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/produto/"))
                {
                    var localProduto = _localSession.Get<FolhaPagamento>(log.Chaves["CodBarra"]);
                    var remoteProduto = _remoteSession.Get<FolhaPagamento>(log.Chaves["CodBarra"]);

                    if (localProduto == null)
                    {
                        _localSession.Replicate(remoteProduto, ReplicationMode.Overwrite);
                    }
                    else if (remoteProduto == null)
                    {
                        _remoteSession.Replicate(localProduto, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/recebimentocartao/"))
                {
                    var loja = _localSession.Get<Loja>(log.Chaves["Loja"]);
                    var operadora = _localSession.Get<OperadoraCartao>(log.Chaves["OperadoraCartao"]);
                    var mes = int.Parse(log.Chaves["Mes"]);
                    var ano = int.Parse(log.Chaves["Ano"]);

                    var recebimento = new RecebimentoCartao()
                    {
                        Loja = loja,
                        OperadoraCartao = operadora,
                        Mes = mes,
                        Ano = ano
                    };

                    var localRecebimentoCartao = _localSession.Get<RecebimentoCartao>(recebimento);
                    var remoteRecebimentoCartao = _remoteSession.Get<RecebimentoCartao>(recebimento);

                    if (localRecebimentoCartao == null)
                    {
                        _localSession.Replicate(remoteRecebimentoCartao, ReplicationMode.Overwrite);
                    }
                    else if (remoteRecebimentoCartao == null)
                    {
                        _remoteSession.Replicate(localRecebimentoCartao, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
                else if (log.Topico.Contains("/tipocontagem/"))
                {
                    var localParcela = _localSession.Get<FolhaPagamento>(log.Chaves["Id"]);
                    var remoteParcela = _remoteSession.Get<FolhaPagamento>(log.Chaves["Id"]);

                    if (localParcela == null)
                    {
                        _localSession.Replicate(remoteParcela, ReplicationMode.Overwrite);
                    }
                    else if (remoteParcela == null)
                    {
                        _remoteSession.Replicate(localParcela, ReplicationMode.Overwrite);
                    }
                    else
                    {
                        //TODO: adicionar a duplicatas
                    }
                }
            }
        }
    }
}
