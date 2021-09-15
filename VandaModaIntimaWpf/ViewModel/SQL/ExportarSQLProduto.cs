using NHibernate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;

namespace VandaModaIntimaWpf.ViewModel.SQL
{
    class ExportarSQLProduto : ExportarSQLViewModel<Model.Produto>
    {
        public ExportarSQLProduto(IList<Model.Produto> produtos, ISession session) : base(produtos, session)
        {
            daoEntidade = new DAOProduto(_session);
            Aliases = GetAliases(new string[] { "Grades" });
        }
        protected override void ExportarSQLInsert(StreamWriter sw, string fileName)
        {
            var aliasesAdicionais = Aliases.Where(p => p.Coluna == null);

            MySQLAliases aliasCodBarra = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("CodBarra"));
            MySQLAliases aliasDescricao = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Descricao"));
            MySQLAliases aliasPreco = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Preco"));
            MySQLAliases aliasPrecoCusto = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("PrecoCusto"));
            MySQLAliases aliasFornecedor = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Fornecedor"));
            MySQLAliases aliasMarca = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Marca"));
            MySQLAliases aliasNcm = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Ncm"));

            foreach (Model.Produto produto in Entidades)
            {
                string campos = $"`{aliasCodBarra.Alias}`, `{aliasDescricao.Alias}`, `{aliasPreco.Alias}`";
                string valores = $"\"{produto.CodBarra}\", \"{produto.Descricao}\"";

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

                foreach (MySQLAliases aliases in aliasesAdicionais)
                {
                    campos += $", `{aliases.Alias}`";
                    valores += $", \"{aliases.ValorPadrao}\"";

                    if (aliases.ValorPadrao.Equals("NOW()"))
                    {
                        valores += $", {aliases.ValorPadrao}";
                    }
                    else
                    {
                        valores += $", \"{aliases.ValorPadrao}\"";
                    }
                }

                sw.WriteLine($"INSERT INTO produto ({campos}) VALUES ({valores});");
            }
        }

        protected override void ExportarSQLUpdate(StreamWriter sw, string fileName)
        {
            var aliasesAdicionais = Aliases.Where(p => p.Coluna == null);

            MySQLAliases aliasCodBarra = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("CodBarra"));
            MySQLAliases aliasDescricao = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Descricao"));
            MySQLAliases aliasPreco = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Preco"));
            MySQLAliases aliasPrecoCusto = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("PrecoCusto"));
            MySQLAliases aliasFornecedor = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Fornecedor"));
            MySQLAliases aliasMarca = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Marca"));
            MySQLAliases aliasNcm = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Ncm"));

            foreach (Model.Produto produto in Entidades)
            {
                bool addVirgula = false;
                string comandoUpdate = "UPDATE produto SET ";

                if (aliasDescricao != null)
                {
                    comandoUpdate += $"{aliasDescricao.Alias} = \"{produto.Descricao}\"";
                    addVirgula = true;
                }

                if (produto.Marca != null && aliasMarca != null)
                {
                    if (addVirgula)
                        comandoUpdate += ", ";

                    comandoUpdate += $"{aliasMarca.Alias} = \"{produto.Marca.Nome}\"";
                    addVirgula = true;
                }

                if (!string.IsNullOrEmpty(produto.Ncm) && aliasNcm != null)
                {
                    if (addVirgula)
                        comandoUpdate += ", ";

                    comandoUpdate += $"{aliasNcm.Alias} = \"{produto.Ncm}\"";
                    addVirgula = true;
                }

                if (produto.Fornecedor != null && aliasFornecedor != null)
                {
                    if (addVirgula)
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

                    addVirgula = true;
                }

                foreach (MySQLAliases aliases in aliasesAdicionais)
                {
                    if (addVirgula)
                        comandoUpdate += ", ";

                    if (aliases.ValorPadrao.Equals("NOW()"))
                    {
                        comandoUpdate += $"{aliases.Alias} = {aliases.ValorPadrao}";
                    }
                    else
                    {
                        comandoUpdate += $"{aliases.Alias} = \"{aliases.ValorPadrao}\"";
                    }

                    addVirgula = true;
                }

                comandoUpdate += $" WHERE {aliasCodBarra.Alias} = \"{produto.CodBarra}\";";

                sw.WriteLine(comandoUpdate);
            }
        }
    }
}
