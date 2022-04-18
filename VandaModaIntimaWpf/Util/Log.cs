using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Util
{
    public class Log
    {
        private static readonly string Pasta = "Logs";
        public static readonly string LogBanco = Path.Combine(Pasta, "LogBanco.txt");

        public static void EscreveLogBanco(Exception ex, string descricao)
        {
            Directory.CreateDirectory(Pasta);

            string msg = $"Descrição:\n{descricao.ToUpper()}\n";
            msg += $"Data/Hora:\n{DateTime.Now}\n";
            msg += $"Mensagem:\n{ex.Message}\n";
            EscreveInnerException(ex.InnerException, ref msg);
            msg += $"StackTrace:\n{ex.StackTrace}\n\n\n";

            Console.WriteLine(msg);

            File.AppendAllText(LogBanco, msg);
        }

        private static void EscreveInnerException(Exception innerException, ref string msg)
        {
            if (innerException != null)
            {
                msg += $"InnerException:\n{innerException.Message}";
                EscreveInnerException(innerException.InnerException, ref msg);
            }
        }
    }
}
