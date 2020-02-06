using SincronizacaoBD.Model;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static IList<EntidadeMySQL<E>> LerXmlLocal()
        {
            IList<EntidadeMySQL<E>> lista = new List<EntidadeMySQL<E>>();

            string Caminho = $@"{Diretorio}\{typeof(E).Name}";

            if (Directory.Exists(Caminho))
            {
                string[] arquivos = Directory.GetFiles(Caminho, "*.xml");

                foreach (string arquivo in arquivos)
                {
                    XmlRootAttribute root = new XmlRootAttribute();
                    root.ElementName = "EntidadeMySQL";
                    var serializer = new XmlSerializer(typeof(EntidadeMySQL<E>), root);

                    using (TextReader reader = new StreamReader(arquivo))
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
                string CaminhoLocal = Path.Combine(Path.GetTempPath(), $@"VandaModaIntima\{CaminhoRemoto.Replace("/", @"\")}");

                TransferOptions transferOptions = new TransferOptions();
                transferOptions.FileMask = $"*xml>={lastUpdate.ToString("yyyy-MM-dd HH:mm:ss")}";

                TransferOperationResult transferOperationResult = ftpsession.GetFiles(CaminhoRemoto, CaminhoLocal, false, transferOptions);

                if (transferOperationResult.IsSuccess)
                {
                    foreach (TransferEventArgs transferEventArgs in transferOperationResult.Transfers)
                    {
                        Console.WriteLine($"Arquivo {transferEventArgs.FileName} Baixado");
                    }

                    string[] arquivos = Directory.GetFiles($@"{CaminhoLocal}", "*.xml");

                    foreach (string arquivo in arquivos)
                    {
                        XmlRootAttribute root = new XmlRootAttribute
                        {
                            ElementName = "EntidadeMySQL"
                        };

                        var serializer = new XmlSerializer(typeof(EntidadeMySQL<E>), root);

                        using (TextReader reader = new StreamReader(arquivo))
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

                if (Directory.Exists(CaminhoLocal))
                {
                    TransferOperationResult transferOperationResult = ftpsession.PutFiles(CaminhoLocal, CaminhoRemoto, false, transferOptions);

                    transferOperationResult.Check();

                    foreach (TransferEventArgs transferEventArgs in transferOperationResult.Transfers)
                    {
                        Console.WriteLine($"Arquivo {transferEventArgs.FileName} Enviado");
                    }
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
