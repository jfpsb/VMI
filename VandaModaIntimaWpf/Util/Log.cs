using Gerencianet.NETCore.SDK;
using Microsoft.Office.Interop.Excel;
using System;
using System.IO;

namespace VandaModaIntimaWpf.Util
{
    public class Log
    {
        private static readonly string AppDocumentsLogsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Íntima", "Logs");
        public static readonly string LogBanco = Path.Combine(AppDocumentsLogsFolder, "LogBanco.txt");
        public static readonly string LogExcel = Path.Combine(AppDocumentsLogsFolder, "LogExcel.txt");
        public static readonly string LogCredenciais = Path.Combine(AppDocumentsLogsFolder, "LogCredenciais.txt");
        public static readonly string LogGn = Path.Combine(AppDocumentsLogsFolder, "GnLog.txt");
        public static readonly string LogExceptionGenerica = Path.Combine(AppDocumentsLogsFolder, "ExceptionGenerica.txt");

        public static void EscreveLogBanco(Exception ex, string descricao)
        {
            Directory.CreateDirectory(AppDocumentsLogsFolder);

            string msg = $"Descrição:\n{descricao.ToUpper()}\n";
            msg += $"Data/Hora:\n{DateTime.Now}\n";
            msg += $"Mensagem:\n{ex.Message}\n";
            EscreveInnerException(ex.InnerException, ref msg);
            msg += $"StackTrace:\n{ex.StackTrace}\n\n\n";

            Console.WriteLine(msg);

            File.AppendAllText(LogBanco, msg);
        }

        public static void EscreveLogExcel(Exception ex, string descricao)
        {
            Directory.CreateDirectory(AppDocumentsLogsFolder);

            string msg = $"Descrição:\n{descricao.ToUpper()}\n";
            msg += $"Data/Hora:\n{DateTime.Now}\n";
            msg += $"Mensagem:\n{ex.Message}\n";
            EscreveInnerException(ex.InnerException, ref msg);
            msg += $"StackTrace:\n{ex.StackTrace}\n\n\n";

            Console.WriteLine(msg);

            File.AppendAllText(LogExcel, msg);
        }

        private static void EscreveInnerException(Exception innerException, ref string msg)
        {
            if (innerException != null)
            {
                msg += $"InnerException:\n{innerException.Message}";
                EscreveInnerException(innerException.InnerException, ref msg);
            }
        }

        public static void EscreveLogCredenciais(Exception ex)
        {
            Directory.CreateDirectory(AppDocumentsLogsFolder);
            string msg = $"Data/Hora: {DateTime.Now};\nMensagem: \n{ex.Message};";
            msg += $"\nStackTrace: \n{ex.StackTrace}\n\n";
            File.AppendAllText(LogCredenciais, msg);
        }

        public static void EscreveLogGn(GnException ex)
        {
            Directory.CreateDirectory(AppDocumentsLogsFolder);
            string msg = $"Data/Hora: {DateTime.Now};\nMensagem: \n{ex.Message};\nErrorType: \n{ex.ErrorType}";
            msg += $"\nStackTrace: \n{ex.StackTrace}\n\n";
            File.AppendAllText(LogGn, msg);
        }

        public static void EscreveExceptionGenerica(Exception ex)
        {
            Directory.CreateDirectory(AppDocumentsLogsFolder);
            string msg = $"Data/Hora: {DateTime.Now};\nMensagem:\n{ex.Message}\n";

            if (ex.InnerException != null)
            {
                msg += $"InnerException:\n{ex.InnerException.Message}\n";
            }

            msg += $"\nStackTrace: \n{ex.StackTrace}\n\n";
            File.AppendAllText(LogExceptionGenerica, msg);
        }
    }
}
