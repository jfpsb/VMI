using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;
using VandaModaIntimaWpf.ViewModel.FolhaPagamento.CalculoBonusMeta;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.Util
{
    public class PesquisarFolhaPagamentoUtil
    {
        public static async Task<IList<Model.FolhaPagamento>> GeraListaDeFolhas(ISession session,
            IList<Model.Funcionario> funcionarios,
            DateTime dataReferencia, ICalculaBonusMeta calculaBonusMeta)
        {
            DAOFolhaPagamento daoFolha = new DAOFolhaPagamento(session);
            DAOParcela daoParcela = new DAOParcela(session);
            DAOBonus daoBonus = new DAOBonus(session);
            DAOBonusMensal daoBonusMensal = new DAOBonusMensal(session);

            ObservableCollection<Model.FolhaPagamento> folhas = new ObservableCollection<Model.FolhaPagamento>();

            foreach (Model.Funcionario funcionario in funcionarios)
            {
                if ((dataReferencia.Month < funcionario.Admissao.Value.Month && dataReferencia.Year == funcionario.Admissao.Value.Year) ||
                        dataReferencia.Year < funcionario.Admissao.Value.Year)
                    continue;

                if (funcionario.Demissao != null &&
                    ((dataReferencia.Month >= funcionario.Demissao.Value.Month && dataReferencia.Year == funcionario.Demissao.Value.Year) ||
                    dataReferencia.Year > funcionario.Demissao.Value.Year))
                    continue;

                Model.FolhaPagamento folha = await daoFolha.ListarPorMesAnoFuncionario(funcionario, dataReferencia.Month, dataReferencia.Year);

                if (folha == null)
                {
                    folha = new Model.FolhaPagamento
                    {
                        Mes = dataReferencia.Month,
                        Ano = dataReferencia.Year,
                        Funcionario = funcionario
                    };
                }

                //Lista as parcelas somente se folha já existe
                var parcelas = await daoParcela.ListarPorFuncionarioMesAno(folha.Funcionario, folha.Mes, folha.Ano);
                folha.Parcelas = parcelas;

                //Lista todos os bônus do funcionário (inclusive bônus cancelados)
                folha.Bonus = await daoBonus.ListarPorFuncionario(funcionario, dataReferencia.Month, dataReferencia.Year);

                //Adiciona bônus
                //Só é feito em folhas abertas porque nas fechadas os bônus mensais e de meta já estão inseridos no BD
                if (!folha.Fechada)
                {
                    //Lista todos os bônus mensais do funcionário
                    var bonusMensais = await daoBonusMensal.ListarPorFuncionario(funcionario);

                    if (bonusMensais != null && bonusMensais.Count > 0)
                    {
                        foreach (var bonusMensal in bonusMensais)
                        {
                            //Checa se o bônus mensal já existe na lista de bônus de funcionário
                            var descricao = bonusMensal.Descricao;
                            if (bonusMensal.PagoEmFolha)
                                descricao += " (PAGO EM FOLHA)";
                            var bonusJaInserido = folha.Bonus.Count > 0 && folha.Bonus.Any(a => a.Descricao.Equals(descricao));

                            if (bonusJaInserido)
                                continue;

                            //Se não existe cria e adiciona o bônus
                            Bonus bonus = new Bonus
                            {
                                Funcionario = funcionario,
                                Data = new DateTime(folha.Ano, folha.Mes, 1),
                                Descricao = bonusMensal.Descricao,
                                Valor = bonusMensal.Valor,
                                MesReferencia = folha.Mes,
                                AnoReferencia = folha.Ano,
                                BonusMensal = true,
                                PagoEmFolha = bonusMensal.PagoEmFolha
                            };

                            if (bonus.PagoEmFolha)
                                bonus.Descricao += " (PAGO EM FOLHA)";

                            folha.Bonus.Add(bonus);
                        }
                    }

                    double valorBonusDeMeta = calculaBonusMeta.RetornaValorDoBonus(folha.TotalVendido, folha.MetaDeVenda);
                    //Insere bônus de meta se houver
                    if (valorBonusDeMeta > 0)
                    {
                        var mesFolha = new DateTime(folha.Ano, folha.Mes, 1);
                        Bonus bonus = new Bonus
                        {
                            Funcionario = funcionario,
                            Data = new DateTime(folha.Ano, folha.Mes, DateTime.DaysInMonth(folha.Ano, folha.Mes)),
                            Descricao = calculaBonusMeta.DescricaoBonus(mesFolha, folha.TotalVendido, folha.MetaDeVenda),
                            Valor = valorBonusDeMeta,
                            MesReferencia = folha.Mes,
                            AnoReferencia = folha.Ano
                        };

                        bonus.Descricao += " (PAGO EM FOLHA)";

                        folha.Bonus.Add(bonus);
                    }
                }

                //Depois da checagem acima, removo os bônus cancelados da listagem
                if (folha.Bonus != null)
                    folha.Bonus = folha.Bonus.Where(w => w.BonusCancelado == false).ToList();

                folhas.Add(folha);
            }

            return folhas;
        }
    }
}
