﻿using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAORecebimentoCartao : DAO<RecebimentoCartao>
    {
        public DAORecebimentoCartao(ISession isession) : base(isession) { }

        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLoja(int mes, int ano, Loja loja)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Mes", mes));
            criteria.Add(Restrictions.Eq("Ano", ano));
            criteria.Add(Restrictions.Eq("Loja", loja));

            return await Listar(criteria);
        }
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLojaBanco(int mes, int ano, Loja loja, Banco banco)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Mes", mes));
            criteria.Add(Restrictions.Eq("Ano", ano));
            criteria.Add(Restrictions.Eq("Loja", loja));
            criteria.Add(Restrictions.Eq("Banco", banco));

            return await Listar(criteria);
        }
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLojaGroupByLoja(int mes, int ano, Loja loja)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Mes", mes));
            criteria.Add(Restrictions.Eq("Ano", ano));
            criteria.Add(Restrictions.Eq("Loja", loja));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Sum("Recebido"), "Recebido")
                .Add(Projections.Sum("ValorOperadora"), "ValorOperadora")
                .Add(Projections.Property("Mes"), "Mes")
                .Add(Projections.Property("Ano"), "Ano")
                .Add(Projections.Property("Loja"), "Loja")
                .Add(Projections.GroupProperty("Loja")));

            criteria.SetResultTransformer(Transformers.AliasToBean<RecebimentoCartao>());

            return await Listar(criteria);
        }
        /// <summary>
        /// Retorna Uma Lista de Recebimento de Cartão Com a Soma do campo Recebido e ValorOperadora Agrupado Por Loja
        /// </summary>
        /// <param name="mes">Mês Do Recebimento de Cartão Desejado</param>
        /// <param name="ano">Ano Do Recebimento de Cartão Desejado</param>
        /// <returns>Lista de Recebimentos de Cartão</returns>
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoGroupByLoja(int mes, int ano)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Mes", mes));
            criteria.Add(Restrictions.Eq("Ano", ano));

            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Sum("Recebido"), "Recebido")
                .Add(Projections.Sum("ValorOperadora"), "ValorOperadora")
                .Add(Projections.Property("Mes"), "Mes")
                .Add(Projections.Property("Ano"), "Ano")
                .Add(Projections.Property("Loja"), "Loja")
                .Add(Projections.GroupProperty("Loja")));

            criteria.SetResultTransformer(Transformers.AliasToBean<RecebimentoCartao>());

            return await Listar(criteria);
        }

        public async override Task<bool> Deletar(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    RecebimentoCartao recebimentoCartao = (RecebimentoCartao)objeto;

                    await session.DeleteAsync("from RecebimentoCartao WHERE Mes = ? AND Ano = ? and Loja = ?",
                        new object[] { recebimentoCartao.Mes, recebimentoCartao.Ano, recebimentoCartao.Loja.Cnpj },
                        new NHibernate.Type.IType[] { NHibernateUtil.Int32, NHibernateUtil.Int32, NHibernateUtil.String });
                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO DELETAR >>> " + ex.Message);
                }

                return false;
            }
        }
    }
}
