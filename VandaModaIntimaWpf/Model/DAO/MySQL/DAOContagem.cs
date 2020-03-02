using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    class DAOContagem : DAOMySQL<Contagem>
    {
        public DAOContagem(ISession session) : base(session)
        {
        }
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
        public async override Task<Contagem> ListarPorId(params object[] id)
        {
            var criteria = CriarCriteria();

            Loja loja = (Loja)id[0];
            DateTime data = (DateTime)id[1];

            criteria.Add(Restrictions.Eq("Loja", loja));
            criteria.Add(Restrictions.Eq("Data", data));

            var result = await Listar(criteria);

            if (result.Count == 0)
                return null;

            return result[0];
        }
    }
}
