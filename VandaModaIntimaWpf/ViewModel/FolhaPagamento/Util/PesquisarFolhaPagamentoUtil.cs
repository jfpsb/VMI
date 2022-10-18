﻿using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento.Util
{
    public class PesquisarFolhaPagamentoUtil
    {
        public static async Task<IList<Model.FolhaPagamento>> GeraListaDeFolhas(ISession session,
            IList<Model.Funcionario> funcionarios,
            DateTime dataReferencia)
        {
            DAOFolhaPagamento daoFolha = new DAOFolhaPagamento(session);
            DAOParcela daoParcela = new DAOParcela(session);
            DAOBonus daoBonus = new DAOBonus(session);
            DAOBonusMensal daoBonusMensal = new DAOBonusMensal(session);

            ObservableCollection<Model.FolhaPagamento> folhas = new ObservableCollection<Model.FolhaPagamento>();

            foreach (Model.Funcionario funcionario in funcionarios)
            {
                Model.FolhaPagamento folha = await daoFolha.ListarPorMesAnoFuncionario(funcionario, dataReferencia.Month, dataReferencia.Year);

                if (folha == null)
                {
                    if (dataReferencia < funcionario.Admissao)
                        continue;

                    if (dataReferencia > funcionario.Demissao)
                        continue;

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

                    //Insere bônus de meta se houver
                    if (folha.ValorDoBonusDeMeta > 0)
                    {
                        var mesFolha = new DateTime(folha.Ano, folha.Mes, 1);
                        Bonus bonus = new Bonus
                        {
                            Funcionario = funcionario,
                            Data = new DateTime(folha.Ano, folha.Mes, DateTime.DaysInMonth(folha.Ano, folha.Mes)),
                            Descricao = $"COMISSÃO DE VENDA - {mesFolha.ToString("MMMM", CultureInfo.GetCultureInfo("pt-BR"))}",
                            Valor = folha.ValorDoBonusDeMeta,
                            MesReferencia = folha.Mes,
                            AnoReferencia = folha.Ano
                        };

                        if (bonus.PagoEmFolha)
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
