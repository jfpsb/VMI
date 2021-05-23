using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOContagem : DAO<Contagem>
    {
        public DAOContagem(ISession session) : base(session) { }
        public async Task<IList<Contagem>> ListarPorLojaEPeriodo(Loja loja, DateTime dataInicial, DateTime dataFinal)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("Loja", loja));
            criteria.Add(Restrictions.Between("Data", dataInicial, dataFinal));
            return await Listar(criteria);
        }
        public async Task<IList<Contagem>> ListarPorTipo(TipoContagem tipoContagem)
        {
            var criteria = CriarCriteria();
            criteria.Add(Restrictions.Eq("TipoContagem", tipoContagem));
            return await Listar(criteria);
        }
    }
}
