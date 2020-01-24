using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAORecebimentoCartao : DAOMySQL<RecebimentoCartao>
    {
        public DAORecebimentoCartao(ISession isession) : base(isession) { }
        public async override Task<RecebimentoCartao> ListarPorId(params object[] id)
        {
            var mes = id[0];
            var ano = id[1];
            var loja = id[2];
            var operadoracartao = id[3];

            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Mes", mes));
            criteria.Add(Restrictions.Eq("Ano", ano));
            criteria.Add(Restrictions.Eq("Loja", loja));
            criteria.Add(Restrictions.Eq("OperadoraCartao", operadoracartao));

            var result = await Listar(criteria);

            if (result.Count == 0)
            {
                return null;
            }

            return result[0];
        }
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLojaSum(int mes, int ano, Loja loja)
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
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoSum(int mes, int ano)
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
        public override async Task<bool> Inserir(IList<RecebimentoCartao> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (RecebimentoCartao t in objetos)
                    {
                        await session.SaveOrUpdateAsync(t);
                    }

                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
    }
}
