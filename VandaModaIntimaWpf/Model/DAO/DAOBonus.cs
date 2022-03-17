using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOBonus : DAO<Bonus>
    {
        public DAOBonus(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Lista os bônus de um funcionário da folha atual e os bônus que são pagos todo mês.
        /// </summary>
        /// <param name="funcionario">Funcionário que terá os bônus pesquisados</param>
        /// <returns></returns>
        public async Task<IList<Bonus>> ListarPorFuncionario(Funcionario funcionario, int mes, int ano)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("MesReferencia", mes))
                .Add(Restrictions.Eq("AnoReferencia", ano))
                .Add(Restrictions.Eq("Funcionario", funcionario));

            return await Listar(criteria);
        }

        public async Task<double> SomaAlimentacaoPorMesAno(DateTime data)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                .Add(Restrictions.Eq("AnoReferencia", data.Year));
            criteria.Add(Restrictions.Like("Descricao", "%ALIMENTAÇÃO%"));
            criteria.Add(Restrictions.Eq("Deletado", false));
            criteria.SetProjection(Projections.Sum("Valor"));
            var result = await criteria.UniqueResultAsync();

            if (result != null)
                return (double)result;

            return 0;
        }

        public async Task<IList<Model.Bonus>> SomaAlimentacaoPorMesAnoPorLojaTrabalho(DateTime data)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Funcionario.LojaTrabalho", "LojaTrabalho");

            criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                .Add(Restrictions.Eq("AnoReferencia", data.Year));
            criteria.Add(Restrictions.Like("Descricao", "%ALIMENTAÇÃO%"));
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Sum("Valor"), "Valor")
                .Add(Projections.GroupProperty("LojaTrabalho"), "LojaTrabalho"));
            var result = await criteria.UniqueResultAsync();

            criteria.SetResultTransformer(Transformers.AliasToBean<Model.Bonus>());

            return await Listar(criteria);
        }

        public async Task<double> SomaPassagemPorMesAno(DateTime data)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                .Add(Restrictions.Eq("AnoReferencia", data.Year));
            criteria.Add(Restrictions.Like("Descricao", "%PASSAGEM%"));
            criteria.Add(Restrictions.Eq("Deletado", false));
            criteria.SetProjection(Projections.Sum("Valor"));
            var result = await criteria.UniqueResultAsync();

            if (result != null)
                return (double)result;

            return 0;
        }

        public async Task<IList<Model.Bonus>> SomaPassagemPorMesAnoPorLojaTrabalho(DateTime data)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Funcionario.LojaTrabalho", "LojaTrabalho");

            criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                .Add(Restrictions.Eq("AnoReferencia", data.Year));
            criteria.Add(Restrictions.Like("Descricao", "%PASSAGEM%"));
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Sum("Valor"), "Valor")
                .Add(Projections.GroupProperty("LojaTrabalho"), "LojaTrabalho"));
            var result = await criteria.UniqueResultAsync();

            criteria.SetResultTransformer(Transformers.AliasToBean<Model.Bonus>());

            return await Listar(criteria);
        }

        public async Task<double> SomaMetaPorMesAno(DateTime data)
        {
            var criteria = CriarCriteria();

            criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                .Add(Restrictions.Eq("AnoReferencia", data.Year));
            criteria.Add(Restrictions.Like("Descricao", "%META%"));
            criteria.Add(Restrictions.Eq("Deletado", false));
            criteria.SetProjection(Projections.Sum("Valor"));
            var result = await criteria.UniqueResultAsync();

            if (result != null)
                return (double)result;

            return 0;
        }

        public async Task<IList<Model.Bonus>> SomaMetaPorMesAnoPorLojaTrabalho(DateTime data)
        {
            var criteria = CriarCriteria();

            criteria.CreateAlias("Funcionario.LojaTrabalho", "LojaTrabalho");

            criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                .Add(Restrictions.Eq("AnoReferencia", data.Year));
            criteria.Add(Restrictions.Like("Descricao", "%META%"));
            criteria.SetProjection(Projections.ProjectionList()
                .Add(Projections.Sum("Valor"), "Valor")
                .Add(Projections.GroupProperty("LojaTrabalho"), "LojaTrabalho"));
            var result = await criteria.UniqueResultAsync();

            criteria.SetResultTransformer(Transformers.AliasToBean<Model.Bonus>());

            return await Listar(criteria);
        }
    }
}
