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

        public async Task<double> RetornaSomaTodasDespesas(DateTime data)
        {
            var criteria = CriarCriteria();
            criteria.Add(Expression.Sql($"YEAR(Data) = ?", data.Year, NHibernateUtil.Int32));
            criteria.Add(Expression.Sql($"MONTH(Data) = ?", data.Month, NHibernateUtil.Int32));
            criteria.SetProjection(Projections.Sum("Valor"));
            return (double)await criteria.UniqueResultAsync();
        }

        public async Task<IList<Despesa>> ListarPorTipoDespesaFiltroMesAno(TipoDespesa tipoDespesa, Loja loja, DateTime data, string filtro, string termo)
        {
            var criteria = CriarCriteria();

            //if (tipoDespesa.Id != 0)
            criteria.Add(Restrictions.Eq("TipoDespesa", tipoDespesa));

            switch (filtro)
            {
                case "Fornecedor":
                    criteria.CreateAlias("Fornecedor", "Fornecedor");
                    AdicionaTermosPesquisa(criteria, "Fornecedor.Nome", termo.Split(new[] { " OU ".ToLower() }, StringSplitOptions.None));
                    //criteria.Add(Restrictions.Like("Fornecedor.Nome", $"%{termo}%"));
                    break;
                case "Descrição":
                    //criteria.Add(Restrictions.Like("Descricao", $"%{termo}%"));
                    AdicionaTermosPesquisa(criteria, "Descricao", termo.Split(new[] { " OU ".ToLower() }, StringSplitOptions.None));
                    break;
                case "Membro Familiar":
                    //criteria.Add(Restrictions.Like("Familiar", $"%{termo}%"));
                    AdicionaTermosPesquisa(criteria, "Familiar", termo.Split(new[] { " OU ".ToLower() }, StringSplitOptions.None));
                    break;
            }

            if (loja.Cnpj != null)
            {
                criteria.Add(Restrictions.Eq("Loja", loja));
            }

            criteria.Add(Expression.Sql($"YEAR(Data) = ?", data.Year, NHibernateUtil.Int32));
            criteria.Add(Expression.Sql($"MONTH(Data) = ?", data.Month, NHibernateUtil.Int32));
            criteria.AddOrder(Order.Asc("Data"));

            return await Listar(criteria);
        }

        private void AdicionaTermosPesquisa(ICriteria criteria, string propertyName, string[] termos)
        {
            var disjunction = Restrictions.Disjunction();

            foreach (string termo in termos)
            {
                disjunction.Add(Restrictions.Like(propertyName, $"%{termo}%"));
            }

            criteria.Add(disjunction);
        }
    }
}
