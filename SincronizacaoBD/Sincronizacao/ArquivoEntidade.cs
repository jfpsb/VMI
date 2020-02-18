using SincronizacaoBD.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WinSCP;

namespace SincronizacaoBD.Sincronizacao
{
    public static class ArquivoEntidade<E> where E : class, IModel
    {
        private static readonly string Diretorio = "EntidadesSalvas";

        public static void EscreverEmXml(EntidadeMySQL<E> entidade)
        {
            XmlTextWriter writer = null;
            try
            {
                if (!Directory.Exists($@"{Diretorio}\{typeof(E).Name}"))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory($@"{Diretorio}\{typeof(E).Name}");
                    directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                var serializer = new XmlSerializer(typeof(EntidadeMySQL<E>));
                writer = new XmlTextWriter($@"{Diretorio}\{typeof(E).Name}\{entidade.EntidadeSalva.GetId()}.xml", Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                serializer.Serialize(writer, entidade);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static IList<EntidadeMySQL<E>> LerXmlLocal(DateTime lastUpdate)
        {
            IList<EntidadeMySQL<E>> lista = new List<EntidadeMySQL<E>>();

            string Caminho = $@"{Diretorio}\{typeof(E).Name}";

            if (Directory.Exists(Caminho))
            {
                var arquivos = new DirectoryInfo($@"{Caminho}").EnumerateFiles("*.xml").Where(arquivo => arquivo.LastWriteTime > lastUpdate);

                foreach (FileInfo arquivo in arquivos)
                {
                    XmlRootAttribute root = new XmlRootAttribute
                    {
                        ElementName = "EntidadeMySQL"
                    };
                    var serializer = new XmlSerializer(typeof(EntidadeMySQL<E>), root);

                    using (TextReader reader = new StreamReader(arquivo.FullName))
                    {
                        EntidadeMySQL<E> entidadeMySQL = (EntidadeMySQL<E>)serializer.Deserialize(reader);
                        lista.Add(entidadeMySQL);
                    }
                }
            }

            return lista;
        }

        public static IList<EntidadeMySQL<E>> LerXmlRemoto(DateTime lastUpdate)
        {
            IList<EntidadeMySQL<E>> lista = new List<EntidadeMySQL<E>>();

            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "ftp.vandamodaintima.com.br",
                UserName = "syncftp@vandamodaintima.com.br",
                Password = "Jfpsb5982jf"
            };

            using (Session ftpsession = new Session())
            {
                ftpsession.Open(sessionOptions);

                string CaminhoRemoto = $"EntidadesSalvas/{typeof(E).Name}";
                string CaminhoLocal = $@"{CaminhoRemoto.Replace("/", @"\")}";

                if (ftpsession.FileExists(CaminhoRemoto))
                {
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.OverwriteMode = OverwriteMode.Overwrite;
                    transferOptions.PreserveTimestamp = true;

                    if (!Directory.Exists(CaminhoLocal))
                        Directory.CreateDirectory(CaminhoLocal);

                    SynchronizationResult synchronizationResult = ftpsession.SynchronizeDirectories(SynchronizationMode.Local, CaminhoLocal, CaminhoRemoto, false, false, SynchronizationCriteria.Time, transferOptions);
                    synchronizationResult.Check();

                    var arquivos = new DirectoryInfo($@"{CaminhoLocal}").EnumerateFiles("*.xml").Where(arquivo => arquivo.LastWriteTime > lastUpdate);

                    XmlRootAttribute root = new XmlRootAttribute
                    {
                        ElementName = "EntidadeMySQL"
                    };

                    var serializer = new XmlSerializer(typeof(EntidadeMySQL<E>), root);

                    foreach (FileInfo arquivo in arquivos)
                    {
                        using (TextReader reader = new StreamReader(arquivo.FullName))
                        {
                            EntidadeMySQL<E> entidadeMySQL = (EntidadeMySQL<E>)serializer.Deserialize(reader);
                            lista.Add(entidadeMySQL);
                        }
                    }
                }
            }

            return lista;
        }

        public static void EnviaXmlRemoto()
        {
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "ftp.vandamodaintima.com.br",
                UserName = "syncftp@vandamodaintima.com.br",
                Password = "Jfpsb5982jf"
            };

            using (Session ftpsession = new Session())
            {
                ftpsession.Open(sessionOptions);

                string CaminhoRemoto = $"{Diretorio}/{typeof(E).Name}";
                string CaminhoLocal = CaminhoRemoto.Replace("/", @"\");

                TransferOptions transferOptions = new TransferOptions();
                transferOptions.OverwriteMode = OverwriteMode.Overwrite;

                if (!ftpsession.FileExists(CaminhoRemoto))
                    ftpsession.CreateDirectory(CaminhoRemoto);

                if (Directory.Exists(CaminhoLocal))
                {
                    SynchronizationResult synchronizationResult = ftpsession.SynchronizeDirectories(SynchronizationMode.Remote, CaminhoLocal, CaminhoRemoto, false, false, SynchronizationCriteria.Time, transferOptions);
                    synchronizationResult.Check();
                }
            }
        }

        public static void DeletaArquivo(string path)
        {
            File.Delete(path);
            Console.WriteLine($"Deletado: {path}");
        }
    }
}
