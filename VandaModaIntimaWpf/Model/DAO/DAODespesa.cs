using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAODespesa : DAO<Model.Despesa>
    {
        public DAODespesa(ISession session) : base(session)
        {
        }

        public async override Task<IList<Despesa>> Listar()
        {
            var criteria = CriarCriteria();
            criteria.AddOrder(Order.Asc("Descricao"));
            return await Listar(criteria);
        }

        public async Task<IList<Despesa>> ListarPorTipoDespesaFiltroMesAno(TipoDespesa tipoDespesa, DateTime data, string filtro, string termo)
        {
            var criteria = CriarCriteria();

            if (tipoDespesa.Id != 0)
                criteria.Add(Restrictions.Eq("TipoDespesa", tipoDespesa));

            switch (filtro)
            {
                case "Fornecedor":
                    criteria.CreateAlias("Fornecedor", "Fornecedor");
                    criteria.Add(Restrictions.Like("Fornecedor.Nome", $"%{termo}%"));
                    break;
                case "Descrição":
                    criteria.Add(Restrictions.Like("Descricao", $"%{termo}%"));
                    break;
                case "Membro Familiar":
                    criteria.Add(Restrictions.Like("Familiar", $"%{termo}%"));
                    break;
            }

            criteria.Add(Expression.Sql($"YEAR(Data) = ?", data.Year, NHibernateUtil.Int32));
            criteria.Add(Expression.Sql($"MONTH(Data) = ?", data.Month, NHibernateUtil.Int32));
            criteria.AddOrder(Order.Asc("Data"));

            return await Listar(criteria);
        }
    }
}
