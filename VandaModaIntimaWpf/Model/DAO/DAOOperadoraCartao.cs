using NHibernate;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOOperadoraCartao : DAO
    {
        public DAOOperadoraCartao(ISession session) : base(session) { }

        /// <summary>
        /// Retorna a Operadora de Cartão
        /// </summary>
        /// <param name="id">Nome da Operadora de Cartão</param>
        /// <returns>Retorna a Operadora de Cartão Encontrada, Senão, Null</returns>
        public override async Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<OperadoraCartao>(id);
        }
    }
}
