using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;

namespace VandaModaIntimaWpf.ViewModel.SQL
{
    class ExportarSQLProduto : ExportarSQLViewModel<Model.Produto>
    {
        public ExportarSQLProduto() : base()
        {
            daoEntidade = new DAOProduto(session);
        }
        protected override void ExportarSQLInsert(StreamWriter sw, IList<Model.Produto> entidades, string fileName)
        {
            var originalAliases = GetAliases();
            var subtracaoAliases = Aliases.Where(p => p.Coluna == null);

            MySQLAliases aliasCodBarra = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Cod_Barra"));
            MySQLAliases aliasDescricao = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Descricao"));
            MySQLAliases aliasPreco = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Preco"));
            MySQLAliases aliasFornecedor = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Fornecedor"));
            MySQLAliases aliasMarca = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Marca"));

            foreach (Model.Produto produto in entidades)
            {
                string campos = $"`{aliasCodBarra.Alias}`, `{aliasDescricao.Alias}`, `{aliasPreco.Alias}`";
                string valores = $"\"{produto.Cod_Barra}\", \"{produto.Descricao}\", \"{produto.Preco}\"";

                if (produto.Fornecedor != null)
                {
                    campos += $", `{aliasFornecedor.Alias}`";

                    if (string.IsNullOrEmpty(aliasFornecedor.ValorPadrao))
                    {
                        valores += $", \"{produto.Fornecedor.Cnpj}\"";
                    }
                    else if (aliasFornecedor.ValorPadrao.Contains("{nao_altere_este_campo}"))
                    {
                        string valorPadrao = aliasFornecedor.ValorPadrao;
                        valorPadrao = valorPadrao.Replace("{nao_altere_este_campo}", produto.Fornecedor.Cnpj);
                        valores += $", {valorPadrao}";
                    }
                }

                if (produto.Marca != null)
                {
                    campos += $", `{aliasMarca.Alias}`";
                    valores += $", \"{produto.Marca.Nome}\"";
                }

                foreach (MySQLAliases aliases in subtracaoAliases)
                {
                    campos += $", `{aliases.Alias}`";
                    valores += $", \"{aliases.ValorPadrao}\"";
                }

                sw.WriteLine($"INSERT INTO produto ({campos}) VALUES ({valores});");
            }
        }

        protected override void ExportarSQLUpdate(StreamWriter sw, IList<Model.Produto> entidades, string fileName)
        {
            MySQLAliases aliasCodBarra = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Cod_Barra"));
            MySQLAliases aliasDescricao = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Descricao"));
            MySQLAliases aliasPreco = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Preco"));
            MySQLAliases aliasFornecedor = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Fornecedor"));
            MySQLAliases aliasMarca = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Marca"));

            foreach (Model.Produto produto in entidades)
            {
                string comandoUpdate;

                comandoUpdate = $"UPDATE produto SET {aliasDescricao.Alias} = \"{produto.Descricao}\", ";
                comandoUpdate += $"{aliasPreco.Alias} = \"{produto.Preco}\"";

                if (produto.Marca != null)
                {
                    comandoUpdate += $", {aliasMarca.Alias} = \"{produto.Marca.Nome}\"";
                }

                if (produto.Fornecedor != null)
                {
                    if (string.IsNullOrEmpty(aliasFornecedor.ValorPadrao))
                    {
                        comandoUpdate += $", {aliasFornecedor.Alias} = \"{produto.Fornecedor.Cnpj}\"";
                    }
                    else if (aliasFornecedor.ValorPadrao.Contains("{nao_altere_este_campo}"))
                    {
                        string valorPadrao = aliasFornecedor.ValorPadrao;
                        valorPadrao = valorPadrao.Replace("{nao_altere_este_campo}", produto.Fornecedor.Cnpj);
                        comandoUpdate += $", {aliasFornecedor.Alias} = {valorPadrao}";
                    }
                }

                comandoUpdate += $" WHERE {aliasCodBarra.Alias} = \"{produto.Cod_Barra}\";";

                sw.WriteLine(comandoUpdate);
            }
        }

        protected override ObservableCollection<MySQLAliases> GetAliases()
        {
            ObservableCollection<MySQLAliases> aliases = new ObservableCollection<MySQLAliases>();
            var persister = SessionProvider.MySessionFactory.GetClassMetadata(typeof(Model.Produto));

            aliases.Add(new MySQLAliases() { Coluna = persister.IdentifierPropertyName, Alias = persister.IdentifierPropertyName });

            foreach (var columnName in persister.PropertyNames)
            {
                aliases.Add(new MySQLAliases() { Coluna = columnName, Alias = columnName });
            }

            return new ObservableCollection<MySQLAliases>(aliases.Where(w => !w.Coluna.Equals("Codigos")));
        }
    }
}
