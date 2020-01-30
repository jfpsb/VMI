using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Model.DAO.MySQL;

namespace VandaModaIntimaWpf.ViewModel.SQL
{
    public class ExportarSQLFornecedor : ExportarSQLViewModel<Model.Fornecedor>
    {
        public ExportarSQLFornecedor() : base()
        {
            daoEntidade = new DAOFornecedor(session);
            Aliases = GetAliases(new string[] { "Produtos" });
        }
        protected override void ExportarSQLInsert(StreamWriter sw, IList<Model.Fornecedor> entidades, string fileName)
        {
            var originalAliases = GetAliases(new string[] { "Produtos" });
            var subtracaoAliases = Aliases.Where(p => p.Coluna == null);

            MySQLAliases aliasCnpj = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Cnpj"));
            MySQLAliases aliasNome = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Nome"));
            MySQLAliases aliasFantasia = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Fantasia"));
            MySQLAliases aliasTelefone = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Telefone"));
            MySQLAliases aliasEmail = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Email"));

            foreach (Model.Fornecedor fornecedor in entidades)
            {
                string campos = $"`{aliasCnpj.Alias}`, `{aliasNome.Alias}`";
                string valores = $"\"{fornecedor.Cnpj}\", \"{fornecedor.Nome}\"";

                if (!string.IsNullOrEmpty(fornecedor.Fantasia))
                {
                    campos += $", `{aliasFantasia.Alias}`";
                    valores += $", \"{fornecedor.Fantasia}\"";
                }

                if (!string.IsNullOrEmpty(fornecedor.Telefone))
                {
                    campos += $", `{aliasTelefone.Alias}`";
                    valores += $", '{new string(fornecedor.Telefone?.Where(c => char.IsDigit(c)).ToArray())}'";
                }

                if (!string.IsNullOrEmpty(fornecedor.Email))
                {
                    campos += $", `{aliasEmail.Alias}`";
                    valores += $", \"{fornecedor.Email}\"";
                }

                foreach (MySQLAliases aliases in subtracaoAliases)
                {
                    campos += $", `{aliases.Alias}`";
                    valores += $", \"{aliases.ValorPadrao}\"";
                }

                sw.WriteLine($"INSERT INTO fornecedor ({campos}) " +
                    $"SELECT * FROM (SELECT {valores}) AS tmp " +
                    $"WHERE NOT EXISTS (SELECT {aliasCnpj.Alias} FROM fornecedor WHERE {aliasCnpj.Alias} = '{fornecedor.Cnpj}');");
            }
        }
        protected override void ExportarSQLUpdate(StreamWriter sw, IList<Model.Fornecedor> entidades, string fileName)
        {
            string aliasCnpj = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Cnpj")).Alias;
            string aliasNome = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Nome")).Alias;
            string aliasFantasia = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Fantasia")).Alias;
            string aliasTelefone = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Telefone")).Alias;
            string aliasEmail = Aliases.Where(w => w.Coluna != null).SingleOrDefault(s => s.Coluna.Equals("Email")).Alias;

            foreach (Model.Fornecedor fornecedor in entidades)
            {
                sw.WriteLine($"UPDATE fornecedor SET {aliasNome} = \"{fornecedor.Nome}\", " +
                    $"{aliasFantasia} = \"{fornecedor.Fantasia}\", " +
                    $"{aliasEmail} = \"{fornecedor.Email}\", " +
                    $"{aliasTelefone} = '{new string(fornecedor.Telefone?.Where(c => char.IsDigit(c)).ToArray())}' WHERE {aliasCnpj} LIKE '{fornecedor.Cnpj}';");
            }
        }
    }
}
