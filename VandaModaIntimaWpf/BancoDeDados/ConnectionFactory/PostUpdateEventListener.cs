using NHibernate;
using NHibernate.Event;
using System;
using System.Threading;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class PostUpdateEventListener : IPostUpdateEventListener
    {
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            PostUpdate(@event);
        }

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            PostUpdate(@event);
            return Task.FromResult(false);
        }

        private async void PostUpdate(PostUpdateEvent @event)
        {
            var entity = @event.Entity;
            var model = entity as AModel;
            var now = DateTime.Now;

            if (entity is Adiantamento)
            {
                var ad = model as Adiantamento;

                if (!model.Deletado)
                    return;

                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var parcs in ad.Parcelas)
                            {
                                parcs.Deletado = true;
                                parcs.ModificadoEm = now;
                                parcs.DeletadoEm = now;
                                await statelessSession.UpdateAsync(parcs);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando parcelas como deletada");
                            throw new Exception($"Erro ao deletar parcelas de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
            else if (entity is CompraDeFornecedor)
            {
                var compra = model as CompraDeFornecedor;
                if (!model.Deletado)
                    return;
                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var arqs in compra.Arquivos)
                            {
                                arqs.Deletado = true;
                                arqs.ModificadoEm = now;
                                arqs.DeletadoEm = now;
                                await statelessSession.UpdateAsync(arqs);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
            else if (entity is Contagem)
            {
                var contagem = entity as Contagem;
                if (!model.Deletado)
                    return;
                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var c in contagem.Contagens)
                            {
                                c.Deletado = true;
                                c.ModificadoEm = now;
                                c.DeletadoEm = now;
                                await statelessSession.UpdateAsync(c);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
            else if (entity is Funcionario)
            {
                var funcionario = entity as Funcionario;

                if (!model.Deletado)
                    return;

                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var a in funcionario.Adiantamentos)
                            {
                                a.Deletado = true;
                                a.ModificadoEm = now;
                                a.DeletadoEm = now;
                                await statelessSession.UpdateAsync(a);
                            }

                            foreach (var a in funcionario.Bonus)
                            {
                                a.Deletado = true;
                                a.ModificadoEm = now;
                                a.DeletadoEm = now;
                                await statelessSession.UpdateAsync(a);
                            }

                            foreach (var a in funcionario.ContasBancarias)
                            {
                                a.Deletado = true;
                                a.ModificadoEm = now;
                                a.DeletadoEm = now;
                                await statelessSession.UpdateAsync(a);
                            }

                            foreach (var a in funcionario.ChavesPix)
                            {
                                a.Deletado = true;
                                a.ModificadoEm = now;
                                a.DeletadoEm = now;
                                await statelessSession.UpdateAsync(a);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
            else if (entity is Grade)
            {
                var grade = entity as Grade;
                if (!model.Deletado)
                    return;
                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var c in grade.ProdutoGrades)
                            {
                                c.Deletado = true;
                                c.ModificadoEm = now;
                                c.DeletadoEm = now;
                                await statelessSession.UpdateAsync(c);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
            else if (entity is EntradaDeMercadoria)
            {
                var entrada = entity as EntradaDeMercadoria;
                if (!model.Deletado)
                    return;
                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var em in entrada.Entradas)
                            {
                                em.Deletado = true;
                                em.ModificadoEm = now;
                                em.DeletadoEm = now;
                                await statelessSession.UpdateAsync(em);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
            else if (entity is Loja)
            {
                var loja = entity as Loja;
                if (!loja.Deletado)
                    return;
                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var c in loja.Aliquotas)
                            {
                                c.Deletado = true;
                                c.ModificadoEm = now;
                                c.DeletadoEm = now;
                                await statelessSession.UpdateAsync(c);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
            else if (entity is ProdutoGrade)
            {
                var pg = entity as ProdutoGrade;
                if (!pg.Deletado)
                    return;
                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var a in pg.SubGrades)
                            {
                                a.Deletado = true;
                                a.ModificadoEm = now;
                                a.DeletadoEm = now;
                                await statelessSession.UpdateAsync(a);
                            }

                            foreach (var a in pg.Historico)
                            {
                                a.Deletado = true;
                                a.ModificadoEm = now;
                                a.DeletadoEm = now;
                                await statelessSession.UpdateAsync(a);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
            else if (entity is Produto)
            {
                var produto = entity as Produto;
                if (!produto.Deletado)
                    return;
                using (IStatelessSession statelessSession = @event.Persister.Factory.OpenStatelessSession())
                {
                    using (ITransaction tx = statelessSession.BeginTransaction())
                    {
                        try
                        {
                            foreach (var a in produto.Grades)
                            {
                                a.Deletado = true;
                                a.ModificadoEm = now;
                                a.DeletadoEm = now;
                                await statelessSession.UpdateAsync(a);
                            }

                            await tx.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await tx.RollbackAsync();
                            Log.EscreveLogBanco(ex, "trigger - marcando arquivos de compra de fornecedor como deletado");
                            throw new Exception($"Erro ao deletar arquivos de compra de fornecedor de adiantamento. Acesse {Log.LogBanco} para mais detalhes", ex);
                        }
                    }
                }
            }
        }
    }
}
