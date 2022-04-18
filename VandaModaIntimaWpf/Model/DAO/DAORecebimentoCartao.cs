using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAORecebimentoCartao : DAO<RecebimentoCartao>
    {
        public DAORecebimentoCartao(ISession isession) : base(isession) { }

        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLoja(int mes, int ano, Loja loja)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Mes", mes));
                criteria.Add(Restrictions.Eq("Ano", ano));
                criteria.Add(Restrictions.Eq("Loja", loja));
                return await Listar(criteria);
            }
            catch(Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar recebimentos por loja");
                throw new Exception($"Erro ao listar recebimentos em conta de cartão. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLojaBanco(int mes, int ano, Loja loja, Banco banco)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("Mes", mes));
                criteria.Add(Restrictions.Eq("Ano", ano));
                criteria.Add(Restrictions.Eq("Loja", loja));
                criteria.Add(Restrictions.Eq("Banco", banco));
                return await Listar(criteria);
            }
            catch(Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar recebimentos por loja banco");
                throw new Exception($"Erro ao listar recebimentos em conta de cartão. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoLojaGroupByLoja(int mes, int ano, Loja loja)
        {
            try
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
            catch(Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar recebimentos por loja");
                throw new Exception($"Erro ao listar recebimentos em conta de cartão. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        /// <summary>
        /// Retorna Uma Lista de Recebimento de Cartão Com a Soma do campo Recebido e ValorOperadora Agrupado Por Loja
        /// </summary>
        /// <param name="mes">Mês Do Recebimento de Cartão Desejado</param>
        /// <param name="ano">Ano Do Recebimento de Cartão Desejado</param>
        /// <returns>Lista de Recebimentos de Cartão</returns>
        public async Task<IList<RecebimentoCartao>> ListarPorMesAnoGroupByLoja(int mes, int ano)
        {
            try
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
            catch(Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar recebimentos por loja");
                throw new Exception($"Erro ao listar recebimentos em conta de cartão. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
