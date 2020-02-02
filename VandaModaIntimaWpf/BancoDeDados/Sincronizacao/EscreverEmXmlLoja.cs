using System.Xml;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    class EscreverEmXmlLoja : IEscreverEmXml<Loja>
    {
        public void EscreverEmBinario(EntidadeMySQL<Loja> entidadeMySQL)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement raiz = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, raiz);
            XmlElement elementoEntidadeMySQL = xmlDocument.CreateElement(string.Empty, "EntidadeMySQL", string.Empty);

            XmlElement elementoEntidadeSalva = xmlDocument.CreateElement(string.Empty, "EntidadeSalva", string.Empty);

            XmlElement elementoCnpj = xmlDocument.CreateElement(string.Empty, "Cnpj", string.Empty);
            XmlElement elementoNome = xmlDocument.CreateElement(string.Empty, "Nome", string.Empty);
            XmlElement elementoTelefone = xmlDocument.CreateElement(string.Empty, "Telefone", string.Empty);
            XmlElement elementoEndereco = xmlDocument.CreateElement(string.Empty, "Endereco", string.Empty);
            XmlElement elementoInscricaoEstadual = xmlDocument.CreateElement(string.Empty, "InscricaoEstadual", string.Empty);

            elementoCnpj.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Cnpj));
            elementoNome.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Nome));
            elementoTelefone.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Telefone));
            elementoEndereco.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Endereco));
            elementoInscricaoEstadual.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.InscricaoEstadual));

            elementoEntidadeSalva.AppendChild(elementoCnpj);
            elementoEntidadeSalva.AppendChild(elementoNome);
            elementoEntidadeSalva.AppendChild(elementoTelefone);
            elementoEntidadeSalva.AppendChild(elementoEndereco);
            elementoEntidadeSalva.AppendChild(elementoInscricaoEstadual);

            if (entidadeMySQL.EntidadeSalva.Matriz != null)
            {
                XmlElement elementoMatriz = xmlDocument.CreateElement(string.Empty, "Matriz", string.Empty);

                XmlElement elementoMatrizCnpj = xmlDocument.CreateElement(string.Empty, "Cnpj", string.Empty);
                XmlElement elementoMatrizNome = xmlDocument.CreateElement(string.Empty, "Nome", string.Empty);
                XmlElement elementoMatrizTelefone = xmlDocument.CreateElement(string.Empty, "Telefone", string.Empty);
                XmlElement elementoMatrizEndereco = xmlDocument.CreateElement(string.Empty, "Endereco", string.Empty);
                XmlElement elementoMatrizInscricaoEstadual = xmlDocument.CreateElement(string.Empty, "InscricaoEstadual", string.Empty);

                elementoMatrizCnpj.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Matriz.Cnpj));
                elementoMatrizNome.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Matriz.Nome));
                elementoMatrizTelefone.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Matriz.Telefone));
                elementoMatrizEndereco.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Matriz.Endereco));
                elementoMatrizInscricaoEstadual.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Matriz.InscricaoEstadual));

                elementoMatriz.AppendChild(elementoMatrizCnpj);
                elementoMatriz.AppendChild(elementoMatrizNome);
                elementoMatriz.AppendChild(elementoMatrizTelefone);
                elementoMatriz.AppendChild(elementoMatrizEndereco);
                elementoMatriz.AppendChild(elementoMatrizInscricaoEstadual);

                elementoEntidadeSalva.AppendChild(elementoMatriz);
            }

            XmlElement elementoTipo = xmlDocument.CreateElement(string.Empty, "OperacaoMySql", string.Empty);
            elementoTipo.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.OperacaoMySql));

            elementoEntidadeMySQL.AppendChild(elementoTipo);
            elementoEntidadeMySQL.AppendChild(elementoEntidadeSalva);
            xmlDocument.AppendChild(elementoEntidadeMySQL);

            xmlDocument.Save($@"EntidadesSalvas\Loja\EntidadeSalva{entidadeMySQL.EntidadeSalva.GetHashCode()}.xml");
        }
    }
}
