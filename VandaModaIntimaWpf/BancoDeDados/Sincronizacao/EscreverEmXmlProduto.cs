using System.Xml;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    public class EscreverEmXmlProduto : IEscreverEmXml<Produto>
    {
        public void EscreverEmBinario(EntidadeMySQL<Produto> entidade)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement raiz = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, raiz);

            XmlElement elementoEntidadeMySQL = xmlDocument.CreateElement(string.Empty, "EntidadeMySQL", string.Empty);

            XmlElement elementoEntidadeSalva = xmlDocument.CreateElement(string.Empty, "EntidadeSalva", string.Empty);
            XmlElement elementoCodBarra = xmlDocument.CreateElement(string.Empty, "Cod_Barra", string.Empty);
            XmlElement elementoDescricao = xmlDocument.CreateElement(string.Empty, "Descricao", string.Empty);
            XmlElement elementoPreco = xmlDocument.CreateElement(string.Empty, "Preco", string.Empty);
            XmlElement elementoNcm = xmlDocument.CreateElement(string.Empty, "Ncm", string.Empty);

            elementoCodBarra.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Cod_Barra));
            elementoDescricao.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Descricao));
            elementoPreco.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Preco.ToString()));
            elementoNcm.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Ncm));

            elementoEntidadeSalva.AppendChild(elementoCodBarra);
            elementoEntidadeSalva.AppendChild(elementoDescricao);
            elementoEntidadeSalva.AppendChild(elementoPreco);
            elementoEntidadeSalva.AppendChild(elementoNcm);

            if (!entidade.OperacaoMySql.Equals("DELETE"))
            {
                if (entidade.EntidadeSalva.Fornecedor != null)
                {
                    XmlElement elementoFornecedor = xmlDocument.CreateElement(string.Empty, "Fornecedor", string.Empty);

                    XmlElement elementoCnpj = xmlDocument.CreateElement(string.Empty, "Cnpj", string.Empty);
                    XmlElement elementoNome = xmlDocument.CreateElement(string.Empty, "Nome", string.Empty);
                    XmlElement elementoFantasia = xmlDocument.CreateElement(string.Empty, "Fantasia", string.Empty);
                    XmlElement elementoEmail = xmlDocument.CreateElement(string.Empty, "Email", string.Empty);
                    XmlElement elementoTelefone = xmlDocument.CreateElement(string.Empty, "Telefone", string.Empty);

                    elementoCnpj.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Fornecedor.Cnpj));
                    elementoNome.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Fornecedor.Nome));
                    elementoFantasia.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Fornecedor.Fantasia));
                    elementoEmail.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Fornecedor.Email));
                    elementoTelefone.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Fornecedor.Telefone));

                    elementoFornecedor.AppendChild(elementoCnpj);
                    elementoFornecedor.AppendChild(elementoNome);
                    elementoFornecedor.AppendChild(elementoFantasia);
                    elementoFornecedor.AppendChild(elementoEmail);
                    elementoFornecedor.AppendChild(elementoTelefone);

                    elementoEntidadeSalva.AppendChild(elementoFornecedor);
                }

                if (entidade.EntidadeSalva.Marca != null)
                {
                    XmlElement elementoMarca = xmlDocument.CreateElement(string.Empty, "Marca", string.Empty);

                    XmlElement elementoMarcaNome = xmlDocument.CreateElement(string.Empty, "Nome", string.Empty);

                    elementoMarcaNome.AppendChild(xmlDocument.CreateTextNode(entidade.EntidadeSalva.Marca.Nome));

                    elementoMarca.AppendChild(elementoMarcaNome);

                    elementoEntidadeSalva.AppendChild(elementoMarca);
                }

                if (entidade.EntidadeSalva.Codigos.Count > 0)
                {
                    XmlElement elementoCodigos = xmlDocument.CreateElement(string.Empty, "Codigos", string.Empty);

                    foreach (string codigo in entidade.EntidadeSalva.Codigos)
                    {
                        XmlElement elementoCodigo = xmlDocument.CreateElement(string.Empty, "Codigo", string.Empty);
                        elementoCodigo.AppendChild(xmlDocument.CreateTextNode(codigo));
                        elementoCodigos.AppendChild(elementoCodigo);
                    }

                    elementoEntidadeSalva.AppendChild(elementoCodigos);
                }
            }

            XmlElement elementoTipo = xmlDocument.CreateElement(string.Empty, "OperacaoMySql", string.Empty);
            elementoTipo.AppendChild(xmlDocument.CreateTextNode(entidade.OperacaoMySql));

            elementoEntidadeMySQL.AppendChild(elementoTipo);
            elementoEntidadeMySQL.AppendChild(elementoEntidadeSalva);
            xmlDocument.AppendChild(elementoEntidadeMySQL);

            xmlDocument.Save($@"EntidadesSalvas\Produto\EntidadeSalva{entidade.EntidadeSalva.GetHashCode()}.xml");
        }
    }
}
