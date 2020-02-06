using NHibernate;
using SincronizacaoBD.Model;
using SincronizacaoBD.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO.MySQL
{
    public abstract class DAOMySQL<T> : IDAO<T> where T : class, IModel, ICloneable
    {
        protected ISession session;
        public DAOMySQL(ISession session)
        {
            this.session = session;
        }

        public async Task<bool> Atualizar(T objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.UpdateAsync(objeto);
                    await transacao.CommitAsync();
                    ArquivoEntidade<T>.EscreverEmXml(new EntidadeMySQL<T>() { OperacaoMySql = "UPDATE", EntidadeSalva = objeto });
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO ATUALIZAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public virtual async Task<bool> Deletar(T objeto)
        {
            string xmlFileName = $"{objeto.GetHashCode()}.xml";

            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    ArquivoEntidade<T>.EscreverEmXml(new EntidadeMySQL<T>() { OperacaoMySql = "DELETE", EntidadeSalva = objeto });
                    await session.DeleteAsync(objeto);
                    await transacao.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    ArquivoEntidade<T>.DeletaArquivo(xmlFileName);
                    Console.WriteLine("ERRO AO DELETAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public virtual async Task<bool> Deletar(IList<T> objetos)
        {
            List<string> xmlFileNames = new List<string>();

            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (T t in objetos)
                    {
                        xmlFileNames.Add($"{t.GetHashCode()}.xml");
                        ArquivoEntidade<T>.EscreverEmXml(new EntidadeMySQL<T>() { OperacaoMySql = "DELETE", EntidadeSalva = t });
                        await session.DeleteAsync(t);
                    }

                    await transacao.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    foreach (string xmlFileName in xmlFileNames)
                    {
                        ArquivoEntidade<T>.DeletaArquivo(xmlFileName);
                    }
                    Console.WriteLine("ERRO AO DELETAR >>> " + ex.Message);
                }

                return false;
            }
        }

        public async Task<bool> Inserir(T objeto)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    await session.SaveAsync(objeto);
                    await transacao.CommitAsync();
                    ArquivoEntidade<T>.EscreverEmXml(new EntidadeMySQL<T>() { OperacaoMySql = "INSERT", EntidadeSalva = objeto });
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO INSERIR >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }

        public virtual async Task<bool> Inserir(IList<T> objetos)
        {
            using (var transacao = session.BeginTransaction())
            {
                try
                {
                    foreach (T t in objetos)
                    {
                        //Testa se a id do objeto sendo adicionado já existe no bd
                        var o = await session.GetAsync<T>(t.GetId());
                        if (o == null)
                        {
                            await session.SaveAsync(t);
                        }
                    }

                    await transacao.CommitAsync();

                    foreach (T t in objetos)
                    {
                        //Testa se a id do objeto sendo adicionado já existe no bd
                        var o = await session.GetAsync<T>(t.GetId());
                        if (o == null)
                        {
                            ArquivoEntidade<T>.EscreverEmXml(new EntidadeMySQL<T>() { OperacaoMySql = "INSERT", EntidadeSalva = t });
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("ERRO AO INSERIR LISTA >>> " + ex.InnerException.Message);
                }

                return false;
            }
        }
        public virtual async Task<IList<T>> Listar()
        {
            try
            {
                var criteria = CriarCriteria();
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                return await criteria.ListAsync<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public async Task<IList<T>> Listar(ICriteria criteria)
        {
            try
            {
                criteria.SetCacheable(true);
                criteria.SetCacheMode(CacheMode.Normal);
                return await criteria.ListAsync<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public ICriteria CriarCriteria()
        {
            return session.CreateCriteria<T>();
        }

        public abstract Task<T> ListarPorId(params object[] id);
    }
}
