using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    class EscreverEmXmlOperadoraCartao : IEscreverEmXml<OperadoraCartao>
    {
        public void EscreverEmBinario(EntidadeMySQL<OperadoraCartao> entidadeMySQL)
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

            if (entidadeMySQL.EntidadeSalva.IdentificadoresBanco.Count > 0)
            {
                XmlElement elementoIdentificadores = xmlDocument.CreateElement(string.Empty, "Identificadores", string.Empty);

                foreach (string identificador in entidadeMySQL.EntidadeSalva.IdentificadoresBanco)
                {
                    XmlElement elementoIdentificador = xmlDocument.CreateElement(string.Empty, "Identificador", string.Empty);
                    elementoIdentificador.AppendChild(xmlDocument.CreateTextNode(identificador));
                    elementoIdentificadores.AppendChild(elementoIdentificador);
                }

                elementoEntidadeSalva.AppendChild(elementoIdentificadores);
            }

            XmlElement elementoTipo = xmlDocument.CreateElement(string.Empty, "OperacaoMySql", string.Empty);
            elementoTipo.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.OperacaoMySql));

            elementoEntidadeMySQL.AppendChild(elementoTipo);
            elementoEntidadeMySQL.AppendChild(elementoEntidadeSalva);
            xmlDocument.AppendChild(elementoEntidadeMySQL);

            xmlDocument.Save($@"EntidadesSalvas\OperadoraCartao\EntidadeSalva{entidadeMySQL.EntidadeSalva.GetHashCode()}.xml");
        }
    }
}
