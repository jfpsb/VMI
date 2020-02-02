using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    class EscreverEmXmlRecebimentoCartao : IEscreverEmXml<RecebimentoCartao>
    {
        public void EscreverEmBinario(EntidadeMySQL<RecebimentoCartao> entidadeMySQL)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement raiz = xmlDocument.DocumentElement;
            xmlDocument.InsertBefore(xmlDeclaration, raiz);
            XmlElement elementoEntidadeMySQL = xmlDocument.CreateElement(string.Empty, "EntidadeMySQL", string.Empty);

            XmlElement elementoEntidadeSalva = xmlDocument.CreateElement(string.Empty, "EntidadeSalva", string.Empty);

            XmlElement elementoMes = xmlDocument.CreateElement(string.Empty, "Mes", string.Empty);
            XmlElement elementoAno = xmlDocument.CreateElement(string.Empty, "Ano", string.Empty);
            XmlElement elementoRecebido = xmlDocument.CreateElement(string.Empty, "Recebido", string.Empty);
            XmlElement elementoValorOperadora = xmlDocument.CreateElement(string.Empty, "ValorOperadora", string.Empty);
            XmlElement elementoObservacao = xmlDocument.CreateElement(string.Empty, "Observacao", string.Empty);

            elementoMes.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Mes.ToString()));
            elementoAno.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Ano.ToString()));
            elementoRecebido.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Recebido.ToString()));
            elementoValorOperadora.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.ValorOperadora.ToString()));
            elementoObservacao.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Observacao));

            XmlElement elementoLoja = xmlDocument.CreateElement(string.Empty, "Loja", string.Empty);
            XmlElement elementoCnpj = xmlDocument.CreateElement(string.Empty, "Cnpj", string.Empty);
            XmlElement elementoNome = xmlDocument.CreateElement(string.Empty, "Nome", string.Empty);
            XmlElement elementoTelefone = xmlDocument.CreateElement(string.Empty, "Telefone", string.Empty);
            XmlElement elementoEndereco = xmlDocument.CreateElement(string.Empty, "Endereco", string.Empty);
            XmlElement elementoInscricaoEstadual = xmlDocument.CreateElement(string.Empty, "InscricaoEstadual", string.Empty);

            elementoCnpj.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Loja.Cnpj));
            elementoNome.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Loja.Nome));
            elementoTelefone.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Loja.Telefone));
            elementoEndereco.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Loja.Endereco));
            elementoInscricaoEstadual.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.Loja.InscricaoEstadual));

            elementoLoja.AppendChild(elementoCnpj);
            elementoLoja.AppendChild(elementoNome);
            elementoLoja.AppendChild(elementoTelefone);
            elementoLoja.AppendChild(elementoEndereco);
            elementoLoja.AppendChild(elementoInscricaoEstadual);

            if (entidadeMySQL.EntidadeSalva.OperadoraCartao != null)
            {
                XmlElement elementoOperadoraCartao = xmlDocument.CreateElement(string.Empty, "Operadora", string.Empty);
                XmlElement elementoOperadoraNome = xmlDocument.CreateElement(string.Empty, "Nome", string.Empty);
                elementoOperadoraNome.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.EntidadeSalva.OperadoraCartao.Nome));
                elementoOperadoraCartao.AppendChild(elementoOperadoraNome);

                if (entidadeMySQL.EntidadeSalva.OperadoraCartao.IdentificadoresBanco.Count > 0)
                {
                    XmlElement elementoIdentificadores = xmlDocument.CreateElement(string.Empty, "Identificadores", string.Empty);

                    foreach (string identificador in entidadeMySQL.EntidadeSalva.OperadoraCartao.IdentificadoresBanco)
                    {
                        XmlElement elementoIdentificador = xmlDocument.CreateElement(string.Empty, "Identificador", string.Empty);
                        elementoIdentificador.AppendChild(xmlDocument.CreateTextNode(identificador));
                        elementoIdentificadores.AppendChild(elementoIdentificador);
                    }

                    elementoOperadoraCartao.AppendChild(elementoIdentificadores);
                }

                elementoEntidadeSalva.AppendChild(elementoOperadoraCartao);
            }

            elementoEntidadeSalva.AppendChild(elementoMes);
            elementoEntidadeSalva.AppendChild(elementoAno);
            elementoEntidadeSalva.AppendChild(elementoRecebido);
            elementoEntidadeSalva.AppendChild(elementoValorOperadora);
            elementoEntidadeSalva.AppendChild(elementoObservacao);
            elementoEntidadeSalva.AppendChild(elementoLoja);

            XmlElement elementoTipo = xmlDocument.CreateElement(string.Empty, "OperacaoMySql", string.Empty);
            elementoTipo.AppendChild(xmlDocument.CreateTextNode(entidadeMySQL.OperacaoMySql));

            elementoEntidadeMySQL.AppendChild(elementoTipo);
            elementoEntidadeMySQL.AppendChild(elementoEntidadeSalva);
            xmlDocument.AppendChild(elementoEntidadeMySQL);

            xmlDocument.Save($@"EntidadesSalvas\RecebimentoCartao\EntidadeSalva{entidadeMySQL.EntidadeSalva.GetHashCode()}.xml");
        }
    }
}
