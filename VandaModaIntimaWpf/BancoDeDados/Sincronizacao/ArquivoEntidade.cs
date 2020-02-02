using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    public class ArquivoEntidade
    {
        public static void EscreverEmBinario(EntidadeMySQL entidade)
        {
            TextWriter writer = null;
            try
            {
                if (!Directory.Exists("EntidadesSalvas"))
                    Directory.CreateDirectory("EntidadesSalvas");

                var serializer = new XmlSerializer(typeof(EntidadeMySQL));
                writer = new StreamWriter($@"EntidadesSalvas\EntidadesSalvas{entidade.EntidadeSalva.GetHashCode()}.bin", false);
                serializer.Serialize(writer, entidade);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static IList<EntidadeMySQL> LerDeBinario()
        {
            TextReader reader = null;
            IList<EntidadeMySQL> lista = new List<EntidadeMySQL>();
            try
            {
                if (Directory.Exists("EntidadesSalvas"))
                {
                    string[] arquivos = Directory.GetFiles("EntidadesSalvas", "*.bin");

                    foreach (string arquivo in arquivos)
                    {
                        var serializer = new XmlSerializer(typeof(EntidadeMySQL));
                        reader = new StreamReader(arquivo);
                        EntidadeMySQL entidadeMySQL = (EntidadeMySQL)serializer.Deserialize(reader);
                        lista.Add(entidadeMySQL);
                    }
                }

                return lista;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
