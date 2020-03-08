using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util.Sincronizacao;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAORecebimentoCartao : DAO
    {
        public DAORecebimentoCartao(ISession isession) : base(isession) { }

        /// <summary>
        /// Retorna o Recebimento De Cartão
        /// </summary>
        /// <param name="id">Objeto do Tipo RecebimentoCartão Com Os Campos Mes, Ano, Loja e OperadoraCartao Preenchidos</param>
        /// <returns>Retorna o Recebimento De Cartão Encontrado, Senão, Null</returns>
        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<RecebimentoCartao>(id);
        }
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLoja(int mes, int ano, Loja loja)
        {
            var criteria = CriarCriteria<RecebimentoCartao>();

            criteria.Add(Restrictions.Eq("Mes", mes));
            criteria.Add(Restrictions.Eq("Ano", ano));
            criteria.Add(Restrictions.Eq("Loja", loja));

            return await Listar<RecebimentoCartao>(criteria);
        }
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLojaGroupByLoja(int mes, int ano, Loja loja)
        {
            var criteria = CriarCriteria<RecebimentoCartao>();

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

            return await Listar<RecebimentoCartao>(criteria);
        }
        /// <summary>
        /// Retorna Uma Lista de Recebimento de Cartão Com a Soma do campo Recebido e ValorOperadora Agrupado Por Loja
        /// </summary>
        /// <param name="mes">Mês Do Recebimento de Cartão Desejado</param>
        /// <param name="ano">Ano Do Recebimento de Cartão Desejado</param>
        /// <returns>Lista de Recebimentos de Cartão</returns>
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoGroupByLoja(int mes, int ano)
        {
            var criteria = CriarCriteria<RecebimentoCartao>();

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

            return await Listar<RecebimentoCartao>(criteria);
        }

        public async override Task<bool> Deletar(object objeto, bool writeToJson = true, bool sendToServer = true)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    RecebimentoCartao recebimentoCartao = (RecebimentoCartao)objeto;

                    await session.DeleteAsync("from RecebimentoCartao WHERE Mes = ? AND Ano = ? and Loja = ?",
                        new object[] { recebimentoCartao.Mes, recebimentoCartao.Ano, recebimentoCartao.Loja.Cnpj },
                        new NHibernate.Type.IType[] { NHibernateUtil.Int32, NHibernateUtil.Int32, NHibernateUtil.String });

                    if (writeToJson)
                    {
                        IList<RecebimentoCartao> recebimentos = await ListarPorMesAnoLoja(recebimentoCartao.Mes, recebimentoCartao.Ano, recebimentoCartao.Loja);

                        foreach (RecebimentoCartao recebimento in recebimentos)
                        {
                            DatabaseLogFile<RecebimentoCartao> databaseLogFile = SincronizacaoViewModel.WriteDatabaseLogFile<RecebimentoCartao>("DELETE", recebimento);

                            if (sendToServer)
                                SincronizacaoViewModel.SendDatabaseLogFileToServer(databaseLogFile);
                        }
                    }

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
