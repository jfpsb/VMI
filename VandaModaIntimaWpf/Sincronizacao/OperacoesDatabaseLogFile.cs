using Newtonsoft.Json;
using System;
using System.IO;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.Sincronizacao
{
    public static class OperacoesDatabaseLogFile<E> where E : class, IModel
    {
        private static readonly string Diretorio = "DatabaseLog";

        public static void EscreverJson(string operacao, E entidade)
        {
            try
            {
                if (!Directory.Exists($"{Diretorio}"))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory($"{Diretorio}");
                    directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                DateTime LastWriteTime = DateTime.Now;
                DatabaseLogFile<E> databaseLogFile = new DatabaseLogFile<E>() { OperacaoMySQL = operacao, Entidade = entidade, LastWriteTime = LastWriteTime };

                string json = JsonConvert.SerializeObject(databaseLogFile, Formatting.Indented);

                File.WriteAllText(Path.Combine(Diretorio, $"{databaseLogFile.GetClassName()} {databaseLogFile.GetIdentifier()}.json"), json);
                File.SetLastWriteTime(Path.Combine(Diretorio, $"{databaseLogFile.GetClassName()} {databaseLogFile.GetIdentifier()}.json"), LastWriteTime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DeletaArquivo(string fileName)
        {
            File.Delete(Path.Combine(Diretorio, fileName));
            Console.WriteLine($"Deletado: {fileName}");
        }
    }
}
