using NHibernate.Driver;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text.RegularExpressions;
using VandaModaIntimaWpf.Model;
using VandaModaIntimaWpf.Util.Sincronizacao;
using VandaModaIntimaWpf.ViewModel;

namespace VandaModaIntimaWpf.BancoDeDados.ConnectionFactory
{
    public class CustomMySqlDataDriver : MySqlDataDriver
    {
        public override void AdjustCommand(DbCommand command)
        {
            if (command.CommandText.StartsWith("INSERT")
                || command.CommandText.StartsWith("UPDATE")
                || command.CommandText.StartsWith("DELETE"))
            {
                StatementLog statementLog = new StatementLog();

                string statement = command.CommandText;
                statementLog.Tabela = GetTableName(statement);

                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    string parametro = $"?p{i}";
                    object valor = command.Parameters[i].Value;

                    string valorEmString;

                    if (valor.ToString().Equals(string.Empty))
                    {
                        valorEmString = "null";
                    }
                    else if (valor.GetType() == typeof(DateTime))
                    {
                        valorEmString = $"'{((DateTime)valor).ToString("yyyy-MM-dd HH:mm:ss.ffffff")}'";
                    }
                    else
                    {
                        valorEmString = $"'{valor.ToString()}'";
                    }

                    statement = statement.Replace(parametro.ToString(), valorEmString);
                }

                statementLog.Statement = statement + ";";
                statementLog.WriteTime = DateTime.Now;
                GetIds(statementLog, command.Parameters);

                if (!SincronizacaoViewModel.TransientWriteStatementLogs.Contains(statementLog))
                {
                    SincronizacaoViewModel.TransientWriteStatementLogs.Add(statementLog);
                }
            }

            base.AdjustCommand(command);
        }

        private string GetTableName(string statement)
        {
            if (statement.StartsWith("INSERT"))
            {
                Regex regex = new Regex(@"(?<=\bINTO\s+)\p{L}+");
                Match match = regex.Match(statement);
                return match.Groups[0].Value;
            }
            else if (statement.StartsWith("UPDATE"))
            {
                Regex regex = new Regex(@"(?<=\bUPDATE\s+)\p{L}+");
                Match match = regex.Match(statement);
                return match.Value;
            }
            else
            {
                Regex regex = new Regex(@"(?<=\bFROM\s+)\p{L}+");
                Match match = regex.Match(statement);
                return match.Value;
            }
        }

        private void GetIds(StatementLog statementLog, DbParameterCollection dbParameterCollection)
        {
            int lastIndex = dbParameterCollection.Count - 1;

            if (statementLog.Tabela.Equals("contagem"))
            {
                string loja, data;

                loja = dbParameterCollection[lastIndex - 1].Value.ToString();
                data = dbParameterCollection[lastIndex].Value.ToString();

                statementLog.Identificadores.Add(loja);
                statementLog.Identificadores.Add(data);
            }
            else if (statementLog.Tabela.Equals("recebimentocartao"))
            {
                string mes, ano, loja, operadora;

                mes = dbParameterCollection[lastIndex - 3].Value.ToString();
                ano = dbParameterCollection[lastIndex - 2].Value.ToString();
                loja = dbParameterCollection[lastIndex - 1].Value.ToString();
                operadora = dbParameterCollection[lastIndex].Value.ToString();

                statementLog.Identificadores.Add(mes);
                statementLog.Identificadores.Add(ano);
                statementLog.Identificadores.Add(loja);
                statementLog.Identificadores.Add(operadora);
            }
            else if (statementLog.Tabela.Equals("codbarrafornecedor"))
            {
                string produto, codigo;

                produto = dbParameterCollection[lastIndex - 1].Value.ToString();
                codigo = dbParameterCollection[lastIndex].Value.ToString();

                statementLog.Identificadores.Add(produto);
                statementLog.Identificadores.Add(codigo);
            }
            else
            {
                string id = dbParameterCollection[lastIndex].Value.ToString();
                statementLog.Identificadores.Add(id);
            }
        }
    }
}
