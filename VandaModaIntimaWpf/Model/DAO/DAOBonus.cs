using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

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
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Eq("MesReferencia", mes))
                    .Add(Restrictions.Eq("AnoReferencia", ano))
                    .Add(Restrictions.Eq("Funcionario", funcionario));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar por funcionário em DAOBonus");
                throw new Exception($"Erro ao listar bônus por funcionário. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<double> SomaAlimentacaoPorMesAno(DateTime data)
        {
            try
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
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "soma de alimentação por mês e ano em DAOBonus");
                throw new Exception($"Erro ao retornar soma de valor de alimentação. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Model.Bonus>> SomaAlimentacaoPorMesAnoPorLojaTrabalho(DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.CreateAlias("Funcionario.LojaTrabalho", "LojaTrabalho");

                criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                    .Add(Restrictions.Eq("AnoReferencia", data.Year));
                criteria.Add(Restrictions.Like("Descricao", "%ALIMENTAÇÃO%"));
                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Property("Funcionario"), "Funcionario")
                    .Add(Projections.Sum("Valor"), "Valor")
                    .Add(Projections.GroupProperty("LojaTrabalho"), "LojaTrabalho"));

                criteria.SetResultTransformer(Transformers.AliasToBean<Model.Bonus>());

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "soma de alimentação por mês e ano agrupado por lojatrabalho em DAOBonus");
                throw new Exception($"Erro ao retornar soma de valor de alimentação por loja de trabalho. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<double> SomaPassagemPorMesAno(DateTime data)
        {
            try
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
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "soma de passagem por mês e ano em DAOBonus");
                throw new Exception($"Erro ao retornar soma de valor de passagem. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Model.Bonus>> SomaPassagemPorMesAnoPorLojaTrabalho(DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.CreateAlias("Funcionario.LojaTrabalho", "LojaTrabalho");

                criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                    .Add(Restrictions.Eq("AnoReferencia", data.Year));
                criteria.Add(Restrictions.Like("Descricao", "%PASSAGEM%"));
                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Property("Funcionario"), "Funcionario")
                    .Add(Projections.Sum("Valor"), "Valor")
                    .Add(Projections.GroupProperty("LojaTrabalho"), "LojaTrabalho"));

                criteria.SetResultTransformer(Transformers.AliasToBean<Model.Bonus>());

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "soma de passagem por mês e ano agrupado por lojatrabalho em DAOBonus");
                throw new Exception($"Erro ao retornar soma de valor de passagem por loja de trabalho. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<double> SomaMetaPorMesAno(DateTime data)
        {
            try
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
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "soma de meta por mês e ano em DAOBonus");
                throw new Exception($"Erro ao retornar soma de valor de meta. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Model.Bonus>> SomaMetaPorMesAnoPorLojaTrabalho(DateTime data)
        {
            try
            {
                var criteria = CriarCriteria();

                criteria.CreateAlias("Funcionario.LojaTrabalho", "LojaTrabalho");

                criteria.Add(Restrictions.Eq("MesReferencia", data.Month))
                    .Add(Restrictions.Eq("AnoReferencia", data.Year));
                criteria.Add(Restrictions.Like("Descricao", "%META%"));
                criteria.SetProjection(Projections.ProjectionList()
                    .Add(Projections.Property("Funcionario"), "Funcionario")
                    .Add(Projections.Sum("Valor"), "Valor")
                    .Add(Projections.GroupProperty("LojaTrabalho"), "LojaTrabalho"));

                criteria.SetResultTransformer(Transformers.AliasToBean<Model.Bonus>());

                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "soma de meta por mês e ano agrupado por lojatrabalho em DAOBonus");
                throw new Exception($"Erro ao retornar soma de valor de meta por loja de trabalho. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
