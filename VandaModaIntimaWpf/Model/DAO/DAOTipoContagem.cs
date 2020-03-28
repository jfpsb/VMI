using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOTipoContagem : DAO
    {
        public DAOTipoContagem(ISession session) : base(session) { }

        public async Task<IList<TipoContagem>> ListarPorNome(string nome)
        {
            var criteria = CriarCriteria<TipoContagem>();

            criteria.Add(Restrictions.Eq("Nome", nome));

            return await Listar<TipoContagem>(criteria);
        }

        /// <summary>
        /// Retorna o Tipo de Contagem
        /// </summary>
        /// <param name="id">Número Identificador do Tipo de Contagem</param>
        /// <returns>Retorna o Tipo de Contagem Encontrado, Senão, Null</returns>
        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<TipoContagem>(id);
        }

        public override int GetMaxId()
        {
            throw new System.NotImplementedException();
        }
    }
}
