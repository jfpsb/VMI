﻿using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAOFuncionario : DAO<Funcionario>
    {
        public DAOFuncionario(ISession session) : base(session) { }

        public async override Task<IList<Funcionario>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar funcionário");
                throw new Exception($"Erro ao listar funcionários. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Funcionario>> Listar(bool mostraDemitido)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.AddOrder(Order.Asc("Nome"));
                if (!mostraDemitido)
                {
                    criteria.Add(Restrictions.IsNull("Demissao"));
                }
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar funcionário");
                throw new Exception($"Erro ao listar funcionários. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Funcionario>> ListarPorNome(string nome, bool mostraDemitido)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Like("Nome", "%" + nome + "%"));
                if (!mostraDemitido)
                {
                    criteria.Add(Restrictions.IsNull("Demissao"));
                }
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar funcionário por nome");
                throw new Exception($"Erro ao listar funcionários por nome. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<IList<Funcionario>> ListarPorCpf(string cpf, bool mostraDemitido)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.Like("Cpf", "%" + cpf + "%"));
                if (!mostraDemitido)
                {
                    criteria.Add(Restrictions.IsNull("Demissao"));
                }
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar funcionário por cpf");
                throw new Exception($"Erro ao listar funcionários por cpf. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Model.Funcionario>> ListarNaoDemitidos()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.IsNull("Demissao"));
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar nao demitidos");
                throw new Exception($"Erro ao listar funcionários não demitidos. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }

        public async Task<IList<Model.Funcionario>> ListarPorLojaTrabalho(Model.Loja loja)
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.Add(Restrictions.IsNull("Demissao"));
                criteria.Add(Restrictions.Eq("LojaTrabalho", loja));
                criteria.AddOrder(Order.Asc("Nome"));
                return await Listar(criteria);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar nao demitidos");
                throw new Exception($"Erro ao listar funcionários não demitidos. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
    }
}
