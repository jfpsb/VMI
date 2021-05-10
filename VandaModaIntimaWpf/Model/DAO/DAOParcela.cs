using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOParcela : DAO
    {
        public DAOParcela(ISession session) : base(session)
        {
        }

        public async Task<IList<Parcela>> ListarPorFuncionarioMesAno(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria<Parcela>();

            criteria.CreateAlias("Adiantamento", "Adiantamento");
            criteria.Add(Restrictions.Eq("Adiantamento.Funcionario", funcionario))
                .Add(Restrictions.Eq("Mes", mes))
                .Add(Restrictions.Eq("Ano", ano));

            return await Listar<Parcela>(criteria);
        }

        public override int GetMaxId()
        {
            throw new NotImplementedException();
        }

        public override Task<object> ListarPorId(object id)
        {
            throw new NotImplementedException();
        }
    }
}
