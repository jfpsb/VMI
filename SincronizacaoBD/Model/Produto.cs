using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Produto : IXmlSerializable, IModel
    {
        private string cod_barra;
        private Fornecedor fornecedor;
        private Marca marca;
        private string descricao;
        private double preco;
        private string ncm;
        private IList<string> codigos = new List<string>();

        public virtual string Cod_Barra
        {
            get
            {
                return cod_barra;
            }
            set
            {
                cod_barra = value;
            }
        }
        public virtual Fornecedor Fornecedor
        {
            get
            {
                return fornecedor;
            }

            set
            {
                fornecedor = value;
            }
        }

        public virtual Marca Marca
        {
            get
            {
                return marca;
            }
            set
            {
                marca = value;
            }
        }

        public virtual string Descricao
        {
            get
            {
                return descricao;
            }

            set
            {
                descricao = value.ToUpper();
            }
        }

        public virtual double Preco
        {
            get { return preco; }
            set
            {
                preco = value;
            }
        }

        public virtual string Ncm
        {
            get { return ncm; }
            set
            {
                ncm = value;
            }
        }
        
        [XmlIgnore]
        public virtual IList<string> Codigos
        {
            get
            {
                return codigos;
            }
            set
            {
                codigos = value;
            }
        }

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Cod_Barra;
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
                        case "Cod_Barra":
                            Cod_Barra = reader.ReadString();
                            break;
                        case "Descricao":
                            Descricao = reader.ReadString();
                            break;
                        case "Preco":
                            Preco = double.Parse(reader.ReadString());
                            break;
                        case "Ncm":
                            Ncm = reader.ReadString();
                            break;
                        case "Codigos":
                            reader.ReadToDescendant("Codigo");
                            do
                            {
                                string codigo = reader.ReadString();
                                Codigos.Add(codigo);
                            } while (reader.ReadToNextSibling("Codigo"));
                            break;
                        case "Fornecedor":
                            XmlReader fornecedorReader = reader.ReadSubtree();

                            Fornecedor = new Fornecedor();

                            fornecedorReader.ReadToFollowing("Cnpj");
                            Fornecedor.Cnpj = fornecedorReader.ReadString();

                            fornecedorReader.ReadToFollowing("Nome");
                            Fornecedor.Nome = fornecedorReader.ReadString();

                            fornecedorReader.ReadToFollowing("Fantasia");
                            Fornecedor.Fantasia = fornecedorReader.ReadString();

                            fornecedorReader.ReadToFollowing("Email");
                            Fornecedor.Email = fornecedorReader.ReadString();

                            fornecedorReader.ReadToFollowing("Telefone");
                            Fornecedor.Telefone = fornecedorReader.ReadString();

                            break;
                        case "Marca":
                            XmlReader marcaReader = reader.ReadSubtree();

                            Marca = new Marca();

                            marcaReader.ReadToFollowing("Nome");
                            Marca.Nome = marcaReader.ReadString();

                            break;
                    }
                }
            }
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            // Campos de produto
            writer.WriteElementString("Cod_Barra", Cod_Barra);
            writer.WriteElementString("Descricao", Descricao);
            writer.WriteElementString("Preco", Preco.ToString());
            writer.WriteElementString("Ncm", Ncm);

            if (Codigos.Count > 0)
            {
                writer.WriteStartElement("Codigos");

                foreach (string codigo in Codigos)
                {
                    writer.WriteElementString("Codigo", codigo);
                }

                writer.WriteEndElement();
            }

            if (Fornecedor != null)
            {
                // Fornecedor de produto
                writer.WriteStartElement("Fornecedor");
                writer.WriteElementString("Cnpj", Fornecedor.Cnpj);
                writer.WriteElementString("Nome", Fornecedor.Nome);
                writer.WriteElementString("Fantasia", Fornecedor.Fantasia);
                writer.WriteElementString("Email", Fornecedor.Email);
                writer.WriteElementString("Telefone", Fornecedor.Telefone);
                writer.WriteEndElement();
            }

            if (Marca != null)
            {
                // Marca
                writer.WriteStartElement("Marca");
                writer.WriteElementString("Nome", Marca.Nome);
                writer.WriteEndElement();
            }
        }
    }
}
