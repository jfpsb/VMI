using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.Model.DAO
{
    public class DAO<E> where E : AModel, IModel
    {
        protected ISession session;
        public DAO(ISession session)
        {
            this.session = session;
        }

        public virtual async Task Inserir(E objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    objeto.Uuid = Guid.NewGuid();
                    var result = await session.SaveAsync(objeto);
                    await transacao.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Log.EscreveLogBanco(ex, "inserir em banco de dados");
                    throw new Exception($"Erro ao inserir em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task Inserir(IList<E> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        e.Uuid = Guid.NewGuid();
                        await session.SaveOrUpdateAsync(e);
                    }

                    await transacao.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Log.EscreveLogBanco(ex, "inserir lista em banco de dados");
                    throw new Exception($"Erro ao inserir lista em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task InserirOuAtualizar(E objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    if (objeto.Uuid == null || objeto.Uuid == Guid.Empty)
                    {
                        objeto.Uuid = Guid.NewGuid();
                        await session.SaveAsync(objeto);
                    }
                    else
                    {
                        await session.UpdateAsync(objeto);
                    }
                    await transacao.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Log.EscreveLogBanco(ex, "inserir ou atualizar em banco de dados");
                    throw new Exception($"Erro ao inserir ou atualizar em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task InserirOuAtualizar(IList<E> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        if (e.Uuid == null || e.Uuid == Guid.Empty)
                        {
                            e.Uuid = Guid.NewGuid();
                            await session.SaveAsync(e);
                        }
                        else
                        {
                            await session.UpdateAsync(e);
                        }
                    }

                    await transacao.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Log.EscreveLogBanco(ex, "inserir ou atualiza lista em banco de dados");
                    throw new Exception($"Erro ao inserir ou atualizar lista em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task Atualizar(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.UpdateAsync(objeto);
                    await transacao.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Log.EscreveLogBanco(ex, "atualizar em banco de dados");
                    throw new Exception($"Erro ao atualizar em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task Merge(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.MergeAsync(objeto);
                    await transacao.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Log.EscreveLogBanco(ex, "merge em banco de dados");
                    throw new Exception($"Erro ao fazer merge em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task Deletar(object objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    AModel model = objeto as AModel;
                    model.Deletado = true;
                    await session.UpdateAsync(model);
                    await transacao.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Log.EscreveLogBanco(ex, "deletar em banco de dados");
                    throw new Exception($"Erro ao deletar em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task Deletar(IList<E> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (E e in objetos)
                    {
                        AModel model = e as AModel;
                        model.Deletado = true;
                        await session.UpdateAsync(model);
                    }

                    await transacao.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transacao.RollbackAsync();
                    Log.EscreveLogBanco(ex, "deletar listar em banco de dados");
                    throw new Exception($"Erro ao deletar lista em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task<E> ListarPorUuid(Guid guid)
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {

                    var criteria = CriarCriteria();
                    criteria.Add(Restrictions.Eq("Uuid", guid.ToString()));
                    criteria.SetCacheable(true);
                    criteria.SetCacheMode(CacheMode.Normal);
                    var result = await criteria.UniqueResultAsync<E>();
                    await tx.CommitAsync();
                    return result;

                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Log.EscreveLogBanco(ex, "listar por uuid em banco de dados");
                    throw new Exception($"Erro ao listar por UUID em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        public virtual async Task<IList<E>> Listar()
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {

                    var criteria = CriarCriteria();
                    criteria.Add(Restrictions.Eq("Deletado", false));
                    criteria.SetCacheable(true);
                    criteria.SetCacheMode(CacheMode.Normal);
                    var results = await criteria.ListAsync<E>();
                    await tx.CommitAsync();
                    return results;

                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    Log.EscreveLogBanco(ex, "listar em banco de dados");
                    throw new Exception($"Erro ao listar em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
                }
            }
        }
        /// <summary>
        /// Retorna Uma Lista De Itens Baseado No Criteria Informado E Que Não Estejam Deletados
        /// </summary>
        /// <param name="criteria">Criteria Para Ser Usado Na Query</param>
        /// <returns>Lista De Itens Do Tipo E</returns>
        public virtual async Task<IList<E>> Listar(ICriteria criteria)
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                criteria.Add(Restrictions.Eq("Deletado", false));
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                var results = await criteria.ListAsync<E>();
                await tx.CommitAsync();
                return results;
            }
        }
        public virtual async Task<IList<E>> ListarComNovaSession(ICriteria criteria)
        {
            try
            {
                using (ISession session = SessionProvider.GetSession())
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        criteria.Add(Restrictions.Eq("Deletado", false));
                        criteria.SetCacheable(true);
                        criteria.SetCacheMode(CacheMode.Normal);
                        var results = await criteria.ListAsync<E>();
                        await tx.CommitAsync();
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar com nova session em banco de dados");
                throw new Exception($"Erro ao listar com nova session em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<E> ListarPorId(object id)
        {
            try
            {
                using (var tx = session.BeginTransaction())
                {
                    var res = await session.GetAsync<E>(id);
                    await tx.CommitAsync();
                    return res;
                }
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar por id em banco de dados");
                throw new Exception($"Erro ao listar por id em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task<long> RetornaMaiorValor(string idProperty)
        {
            try
            {
                var criteria = session.CreateCriteria<E>();
                criteria.SetProjection(Projections.Max(idProperty));
                return await criteria.UniqueResultAsync<long>();
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "listar maior valor em banco de dados");
                throw new Exception($"Erro ao listar maior valor em banco de dados. Acesse {Log.LogBanco} para mais detalhes", ex);
            }
        }
        public async Task RefreshEntidade(object obj)
        {
            try
            {
                await session.RefreshAsync(obj);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "refresh");
                throw new Exception($"Erro ao dar refresh em entidade em banco de dados local. Acesse {Log.LogBanco} para mais detalhes.", ex);
            }
        }
        public async Task RefreshEntidade(IList<E> objs)
        {
            try
            {
                foreach (var obj in objs)
                    await session.RefreshAsync(obj);
            }
            catch (Exception ex)
            {
                Log.EscreveLogBanco(ex, "refresh");
                throw new Exception($"Erro ao dar refresh em lista de entidades em banco de dados local. Acesse {Log.LogBanco} para mais detalhes.", ex);
            }
        }
        public ICriteria CriarCriteria()
        {
            return session.CreateCriteria<E>();
        }

        public ICriteria CriarCriteria(string alias)
        {
            return session.CreateCriteria<E>(alias);
        }
    }
}
