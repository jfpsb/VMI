using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    public class ArquivoEntidade<E> where E : class, IModel, ICloneable
    {
        private IEscreverEmXml<E> escreverEmXml;

        public ArquivoEntidade()
        {
            if (typeof(E) == typeof(Fornecedor))
            {
                escreverEmXml = (IEscreverEmXml<E>)new EscreverEmXmlFornecedor();
            }
            else if (typeof(E) == typeof(Loja))
            {
                escreverEmXml = (IEscreverEmXml<E>)new EscreverEmXmlLoja();
            }
            else if (typeof(E) == typeof(Marca))
            {
                escreverEmXml = (IEscreverEmXml<E>)new EscreverEmXmlMarca();
            }
            else if (typeof(E) == typeof(OperadoraCartao))
            {
                escreverEmXml = (IEscreverEmXml<E>)new EscreverEmXmlOperadoraCartao();
            }
            else if (typeof(E) == typeof(Produto))
            {
                escreverEmXml = (IEscreverEmXml<E>)new EscreverEmXmlProduto();
            }
            else if (typeof(E) == typeof(RecebimentoCartao))
            {
                escreverEmXml = (IEscreverEmXml<E>)new EscreverEmXmlRecebimentoCartao();
            }
        }

        public void EscreverEmBinario(EntidadeMySQL<E> entidade)
        {
            TextWriter writer = null;
            try
            {
                if (!Directory.Exists($@"EntidadesSalvas\{typeof(E).Name}"))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory($@"EntidadesSalvas\{typeof(E).Name}");
                    directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                escreverEmXml.EscreverEmBinario(entidade);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public IList<EntidadeMySQL<E>> LerDeBinario()
        {
            TextReader reader = null;
            IList<EntidadeMySQL<E>> lista = new List<EntidadeMySQL<E>>();
            try
            {
                if (Directory.Exists("EntidadesSalvas"))
                {
                    string[] arquivos = Directory.GetFiles($@"EntidadesSalvas\{typeof(E).Name}", "*.xml");

                    foreach (string arquivo in arquivos)
                    {
                        XmlRootAttribute root = new XmlRootAttribute();
                        root.ElementName = "EntidadeMySQL";
                        var serializer = new XmlSerializer(typeof(EntidadeMySQL<E>), root);
                        reader = new StreamReader(arquivo);
                        EntidadeMySQL<E> entidadeMySQL = (EntidadeMySQL<E>)serializer.Deserialize(reader);
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

        public void EsvaziaDiretorio()
        {
            Directory.Delete("EntidadesSalvas", true);
        }
    }
}
