using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VandaModaIntimaWpf.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class OperadoraCartao : ObservableObject, ICloneable, IModel, IXmlSerializable
    {
        private string nome;
        private DateTime lastUpdate { get; set; } = DateTime.Now;
        private IList<string> identificadoresBanco = new List<string>();
        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public virtual DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        [XmlIgnore]
        public virtual IList<string> IdentificadoresBanco
        {
            get { return identificadoresBanco; }
            set
            {
                identificadoresBanco = value;
                OnPropertyChanged("IdentificadoresBanco");
            }
        }
        public virtual string GetContextMenuHeader { get => Nome; }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetId()
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
                        case "LastUpdate":
                            LastUpdate = XmlConvert.ToDateTime(reader.ReadString(), XmlDateTimeSerializationMode.Unspecified);
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
            writer.WriteElementString("LastUpdate", XmlConvert.ToString(LastUpdate, XmlDateTimeSerializationMode.Unspecified));

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
