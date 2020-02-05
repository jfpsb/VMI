using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SincronizacaoBD.Sincronizacao
{
    public static class ArquivoEntidade<E> where E : class
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
                writer = new XmlTextWriter($@"{Diretorio}\{typeof(E).Name}\{entidade.EntidadeSalva.GetHashCode()}.xml", Encoding.UTF8);
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

        public static IList<EntidadeMySQL<E>> LerDeBinario()
        {
            IList<EntidadeMySQL<E>> lista = new List<EntidadeMySQL<E>>();
            try
            {
                if (Directory.Exists(Diretorio))
                {
                    string[] arquivos = Directory.GetFiles($@"{Diretorio}\{typeof(E).Name}", "*.xml");

                    foreach (string arquivo in arquivos)
                    {
                        XmlRootAttribute root = new XmlRootAttribute();
                        root.ElementName = "EntidadeMySQL";
                        var serializer = new XmlSerializer(typeof(EntidadeMySQL<E>), root);

                        using(TextReader reader = new StreamReader(arquivo))
                        {
                            EntidadeMySQL<E> entidadeMySQL = (EntidadeMySQL<E>)serializer.Deserialize(reader);
                            lista.Add(entidadeMySQL);
                        }
                    }
                }

                return lista;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
                return null;
            }
        }

        public static void DeletaArquivo(string path)
        {
            File.Delete(path);
            Console.WriteLine($"Deletado: {path}");
        }
    }
}
