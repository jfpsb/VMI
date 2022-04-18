using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public class DAOProduto : DAO<Produto>
    {
        public DAOProduto(ISession session) : base(session)
        {
        }

        public async Task<IList<Produto>> ListarPorDescricao(string descricao)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
                criteria.Add(Restrictions.Like("Descricao", "%" + descricao + "%"));
                criteria.AddOrder(Order.Asc("Descricao"));
                criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());
                return await ListarComNovaSession(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar produtos por descricao");
                throw new Exception($"Erro ao listar produtos por descrição. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Produto>> ListarPorDescricaoCodigoDeBarra(string termo)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
                criteria.Add(Restrictions.Disjunction()
                    .Add(Restrictions.Like("CodBarra", "%" + termo + "%"))
                    .Add(Restrictions.Like("Grades.CodBarra", "%" + termo + "%"))
                    .Add(Restrictions.Like("Grades.CodBarraAlternativo", "%" + termo + "%"))
                    .Add(Restrictions.Like("Descricao", "%" + termo + "%")));

                criteria.AddOrder(Order.Asc("Descricao"));
                criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());

                return await ListarComNovaSession(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar produtos por descricao codigo de barra");
                throw new Exception($"Erro ao listar produtos por descrição e/ou código de barras. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Produto>> ListarPorCodigoDeBarra(string codigo)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
                criteria.Add(Restrictions.Like("CodBarra", "%" + codigo + "%"));
                criteria.AddOrder(Order.Asc("CodBarra"));
                criteria.SetResultTransformer(new DistinctRootEntityResultTransformer());
                return await ListarComNovaSession(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar produtos por codigo de barra");
                throw new Exception($"Erro ao listar produtos por código de barras. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<Produto> ListarPorCodigoDeBarraUnico(string codigo)
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    var criteria = CriarCriteria();
                    criteria.Add(Restrictions.Like("CodBarra", codigo));
                    criteria.Add(Restrictions.Eq("Deletado", false));
                    return await criteria.UniqueResultAsync<Produto>();
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Log.EscreveLogBanco(ex, "listar produto por código");
                    throw new Exception($"Erro ao listar produto por código. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public async Task<IList<Produto>> ListarPorFornecedor(string fornecedor)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Grades", "Grades", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
                criteria.CreateAlias("Fornecedor", "Fornecedor");
                criteria.Add(Restrictions.Disjunction()
                    .Add(Restrictions.Like("Fornecedor.Nome", "%" + fornecedor + "%"))
                    .Add(Restrictions.Like("Fornecedor.Fantasia", "%" + fornecedor + "%")));
                return await ListarComNovaSession(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar produtos por fornecedor");
                throw new Exception($"Erro ao listar produtos por fornecedor. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Produto>> ListarPorMarca(string marca)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.CreateAlias("Marca", "Marca");
                criteria.Add(Restrictions.Like("Marca.Nome", "%" + marca + "%"));
                return await ListarComNovaSession(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar produtos por marca");
                throw new Exception($"Erro ao listar produtos por marca. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
