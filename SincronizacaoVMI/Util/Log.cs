﻿using System;
using System.IO;

namespace SincronizacaoVMI.Util
{
    public class Log
    {
        private static readonly string AppDocumentsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Vanda Moda Intima");
        public static readonly string LogSync = Path.Combine(AppDocumentsFolder, "Logs", "SyncLog.txt");
        public static readonly string LogCredenciais = Path.Combine(AppDocumentsFolder, "Logs", "CredenciaisLog.txt");
        public static readonly string LogExceptionGenerica = Path.Combine(AppDocumentsFolder, "Logs", "ExceptionGenerica.txt");
        public static void EscreveLogSync(Exception ex, string operacao)
        {
            Directory.CreateDirectory(Path.Combine(AppDocumentsFolder, "Logs"));
            string msg = $"Operação: {operacao.ToUpper()};\nData/Hora: {DateTime.Now};\nMensagem: \n{ex.Message}";
            if (ex.InnerException != null)
            {
                msg += $"\nInnerException: {ex.InnerException.Message}";
            }
            msg += $"\nStackTrace: \n{ex.StackTrace}\n\n";
            File.AppendAllText(LogSync, msg);
        }

        public static void EscreveExceptionGenerica(Exception ex)
        {
            Directory.CreateDirectory(Path.Combine(AppDocumentsFolder, "Logs"));
            string msg = $"Data/Hora: {DateTime.Now};\nMensagem:\n{ex.Message}\n";
            if (ex.InnerException != null)
            {
                msg += $"InnerException:\n{ex.InnerException.Message}\n";
            }

            msg += $"\nStackTrace: \n{ex.StackTrace}\n\n";
            File.AppendAllText(LogExceptionGenerica, msg);
        }

        public static void EscreveLogCredenciais(Exception ex)
        {
            Directory.CreateDirectory(Path.Combine(AppDocumentsFolder, "Logs"));
            string msg = $"Data/Hora: {DateTime.Now};\nMensagem: \n{ex.Message};";
            msg += $"\nStackTrace: \n{ex.StackTrace}\n\n";
            File.AppendAllText(LogCredenciais, msg);
        }
    }
}
