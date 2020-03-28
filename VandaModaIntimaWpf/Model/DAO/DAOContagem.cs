using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOContagem : DAO
    {
        public DAOContagem(ISession session) : base(session) { }
        public async Task<IList<Contagem>> ListarPorLojaEPeriodo(Loja loja, DateTime dataInicial, DateTime dataFinal)
        {
            var criteria = CriarCriteria<Contagem>();

            criteria.Add(Restrictions.Eq("Loja", loja));
            criteria.Add(Restrictions.Between("Data", dataInicial, dataFinal));

            return await Listar<Contagem>(criteria);
        }
        public async Task<IList<Contagem>> ListarPorTipo(TipoContagem tipoContagem)
        {
            var criteria = CriarCriteria<Contagem>();

            criteria.Add(Restrictions.Eq("TipoContagem", tipoContagem));

            return await Listar<Contagem>(criteria);
        }
        /// <summary>
        /// Pesquisa a Contagem baseado na Id Informada
        /// </summary>
        /// <param name="id">Objeto Do Tipo Contagem Com Os Campos Loja e Data Preenchidos</param>
        /// <returns>Retorna Contagem encontrada, senão, null</returns>
        public async override Task<object> ListarPorId(object id)
        {
            return await session.GetAsync<Contagem>(id);
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }
    }
}
