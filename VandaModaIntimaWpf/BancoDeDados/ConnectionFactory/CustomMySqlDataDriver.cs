using NHibernate.Driver;
using System;
using System.Data.Common;
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
                string statement = command.CommandText;

                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    string parametro = $"?p{i}";
                    object valor = command.Parameters[i].Value;

                    string valorEmString;
                    if (valor.GetType() == typeof(DateTime))
                    {
                        valorEmString = $"'{((DateTime)valor).ToString("yyyy-MM-ddTHH:mm:ss.ffffff")}'";
                    }
                    else
                    {
                        valorEmString = $"'{command.Parameters[i].Value.ToString()}'";
                    }

                    statement = statement.Replace(parametro.ToString(), valorEmString);
                }

                if (!SincronizacaoViewModel.TransientWriteStatementLogs.Contains(new StatementLog() { Statement = statement }))
                {
                    var now = DateTime.Now;
                    SincronizacaoViewModel.TransientWriteStatementLogs.Add(new StatementLog() { Statement = statement, WriteTime = now });
                    SincronizacaoViewModel.TransientSendStatementLogs.Add(new StatementLog() { Statement = statement, WriteTime = now });
                }
            }
            base.AdjustCommand(command);
        }
    }
}
