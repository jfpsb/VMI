using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOProdutoGrade : DAO
    {
        public DAOProdutoGrade(ISession session) : base(session) { }
        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public async override Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<ProdutoGrade>(id);
        }

        public async Task DeletarPorProduto(Produto produto)
        {
            ICriteria criteria = CriarCriteria<ProdutoGrade>();
            criteria.Add(Restrictions.Eq("Produto", produto));
            IList<ProdutoGrade> result = await Listar<ProdutoGrade>(criteria);

            await Deletar(result);
        }
    }
}
