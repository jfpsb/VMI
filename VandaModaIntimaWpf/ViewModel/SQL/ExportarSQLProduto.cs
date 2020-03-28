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
            Aliases = GetAliases(new string[] { "Codigos" });
        }
        protected override void ExportarSQLInsert(StreamWriter sw, IList<Model.Produto> entidades, string fileName)
        {
            var originalAliases = GetAliases(new string[] { "Codigos" });
            var subtracaoAliases = Aliases.Where(p => p.Coluna == null);

            MySQLAliases aliasCodBarra = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("CodBarra"));
            MySQLAliases aliasDescricao = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Descricao"));
            MySQLAliases aliasPreco = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Preco"));
            MySQLAliases aliasFornecedor = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Fornecedor"));
            MySQLAliases aliasMarca = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Marca"));
            MySQLAliases aliasNcm = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Ncm"));

            foreach (Model.Produto produto in entidades)
            {
                string campos = $"`{aliasCodBarra.Alias}`, `{aliasDescricao.Alias}`, `{aliasPreco.Alias}`";
                string valores = $"\"{produto.CodBarra}\", \"{produto.Descricao}\", \"{produto.Preco}\"";

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

                if (!string.IsNullOrEmpty(produto.Ncm))
                {
                    campos += $", `{aliasNcm.Alias}`";
                    valores += $", \"{produto.Ncm}\"";
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
            MySQLAliases aliasCodBarra = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("CodBarra"));
            MySQLAliases aliasDescricao = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Descricao"));
            MySQLAliases aliasPreco = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Preco"));
            MySQLAliases aliasFornecedor = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Fornecedor"));
            MySQLAliases aliasMarca = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Marca"));
            MySQLAliases aliasNcm = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Ncm"));

            foreach (Model.Produto produto in entidades)
            {
                string comandoUpdate = "UPDATE produto SET ";

                if (aliasDescricao != null)
                {
                    comandoUpdate += $"{aliasDescricao.Alias} = \"{produto.Descricao}\"";
                }

                if (aliasPreco != null)
                {
                    if (aliasDescricao != null)
                        comandoUpdate += ", ";

                    comandoUpdate += $"{aliasPreco.Alias} = \"{produto.Preco}\"";
                }

                if (produto.Marca != null && aliasMarca != null)
                {
                    if (aliasDescricao != null || aliasPreco != null)
                        comandoUpdate += ", ";

                    comandoUpdate += $"{aliasMarca.Alias} = \"{produto.Marca.Nome}\"";
                }

                if (!string.IsNullOrEmpty(produto.Ncm) && aliasNcm != null)
                {
                    if (aliasDescricao != null || aliasPreco != null || aliasMarca != null)
                        comandoUpdate += ", ";

                    comandoUpdate += $"{aliasNcm.Alias} = \"{produto.Ncm}\"";
                }

                if (produto.Fornecedor != null && aliasFornecedor != null)
                {
                    if (produto.Marca != null || aliasPreco != null || aliasDescricao != null || aliasNcm != null)
                        comandoUpdate += ", ";

                    if (string.IsNullOrEmpty(aliasFornecedor.ValorPadrao))
                    {
                        comandoUpdate += $"{aliasFornecedor.Alias} = \"{produto.Fornecedor.Cnpj}\"";
                    }
                    else if (aliasFornecedor.ValorPadrao.Contains("{nao_altere_este_campo}"))
                    {
                        string valorPadrao = aliasFornecedor.ValorPadrao;
                        valorPadrao = valorPadrao.Replace("{nao_altere_este_campo}", produto.Fornecedor.Cnpj);
                        comandoUpdate += $"{aliasFornecedor.Alias} = {valorPadrao}";
                    }
                }

                comandoUpdate += $" WHERE {aliasCodBarra.Alias} = \"{produto.CodBarra}\";";

                sw.WriteLine(comandoUpdate);
            }
        }
    }
}
