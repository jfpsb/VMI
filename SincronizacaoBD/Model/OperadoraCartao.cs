using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class OperadoraCartao : IXmlSerializable, IModel
    {
        private string nome;
        private IList<string> identificadoresBanco = new List<string>();
        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
            }
        }

        [XmlIgnore]
        public virtual IList<string> IdentificadoresBanco
        {
            get { return identificadoresBanco; }
            set
            {
                identificadoresBanco = value;
            }
        }

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Nome;
        }

        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "Nome":
                            Nome = reader.ReadString();
                            break;
                        case "IdentificadoresBanco":
                            reader.ReadToDescendant("Identificador");
                            do
                            {
                                string identificador = reader.ReadString();
                                IdentificadoresBanco.Add(identificador);
                            } while (reader.ReadToNextSibling("Identificador"));
                            break;
                    }
                }
            }
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Nome", Nome);

            if (IdentificadoresBanco.Count > 0)
            {
                writer.WriteStartElement("IdentificadoresBanco");

                foreach (string identificador in IdentificadoresBanco)
                {
                    writer.WriteElementString("Identificador", identificador);
                }

                writer.WriteEndElement();
            }
        }
    }
}
