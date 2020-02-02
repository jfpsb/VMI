using System.Xml;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    public class EscreverEmXmlMarca : IEscreverEmXml<Marca>
    {
        public void EscreverEmBinario(EntidadeMySQL<Marca> entidadeMySQL)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement raiz = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, raiz);
            XmlElement elementoEntidadeMySQL = xmlDocument.CreateElement(string.Empty, "EntidadeMySQL", string.Empty);

            XmlElement elementoEntidadeSalva = xmlDocument.CreateElement(string.Empty, "EntidadeSalva", string.Empty);

            XmlElement elementoNome = xmlDocument.CreateElement(string.Empty, "Nome", string.Empty);

            elementoNome.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Nome));

            elementoEntidadeSalva.AppendChild(elementoNome);

            XmlElement elementoTipo = xmlDocument.CreateElement(string.Empty, "OperacaoMySql", string.Empty);
            elementoTipo.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.OperacaoMySql));

            elementoEntidadeMySQL.AppendChild(elementoTipo);
            elementoEntidadeMySQL.AppendChild(elementoEntidadeSalva);
            xmlDocument.AppendChild(elementoEntidadeMySQL);

            xmlDocument.Save($@"EntidadesSalvas\Marca\EntidadeSalva{entidadeMySQL.EntidadeSalva.GetHashCode()}.xml");
        }
    }
}
