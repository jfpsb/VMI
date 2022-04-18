using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOContagemProduto : DAO<ContagemProduto>
    {
        public DAOContagemProduto(ISession session) : base(session) { }

        public async Task<IList<ContagemProduto>> ListarPorContagemGroupByProduto(Contagem contagem)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.Add(Restrictions.Eq("Contagem", contagem));

                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Sum("Quant"), "Quant")
                    .Add(Projections.Property("Id"), "Id")
                    .Add(Projections.Property("Contagem"), "Contagem")
                    .Add(Projections.GroupProperty("Produto"), "Produto"));

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listagem de contagemproduto por produto DAOContagemProduto");
                throw new Exception($"Erro ao listar contagen de produto por produto. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
