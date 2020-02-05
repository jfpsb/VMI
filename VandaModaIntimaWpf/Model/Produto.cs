using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor;
using MarcaModel = VandaModaIntimaWpf.Model.Marca;

namespace VandaModaIntimaWpf.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Produto : ObservableObject, ICloneable, IModel, IXmlSerializable
    {
        private string cod_barra;
        private FornecedorModel fornecedor;
        private MarcaModel marca;
        private string descricao;
        private double preco;
        private string ncm;
        private DateTime lastUpdate { get; set; } = DateTime.Now;
        private IList<string> codigos = new List<string>();
        public enum Colunas
        {
            CodBarra = 1,
            Descricao = 2,
            Preco = 3,
            Fornecedor = 4,
            Marca = 5,
            Ncm = 6,
            CodBarraFornecedor = 7
        }

        public virtual string Cod_Barra
        {
            get
            {
                return cod_barra;
            }
            set
            {
                cod_barra = value;
                OnPropertyChanged("Cod_Barra");
            }
        }
        public virtual FornecedorModel Fornecedor
        {
            get
            {
                return fornecedor;
            }

            set
            {
                fornecedor = value;
                OnPropertyChanged("Fornecedor");
                OnPropertyChanged("FornecedorNome");
            }
        }

        public virtual MarcaModel Marca
        {
            get
            {
                return marca;
            }
            set
            {
                marca = value;
                OnPropertyChanged("Marca");
                OnPropertyChanged("MarcaNome");
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
                OnPropertyChanged("Descricao");
            }
        }

        public virtual double Preco
        {
            get { return preco; }
            set
            {
                preco = value;
                OnPropertyChanged("Preco");
            }
        }

        public virtual string Ncm
        {
            get { return ncm; }
            set
            {
                ncm = value;
                OnPropertyChanged("Ncm");
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
        public virtual IList<string> Codigos
        {
            get
            {
                return codigos;
            }
            set
            {
                codigos = value;
                OnPropertyChanged("Codigos");
            }
        }

        public virtual string FornecedorNome
        {
            get
            {
                if (Fornecedor != null)
                    return Fornecedor.Nome;

                return "Não Há Fornecedor";
            }
        }

        public virtual string MarcaNome
        {
            get
            {
                if (Marca != null)
                    return Marca.Nome;

                return "Não Há Marca";
            }
        }

        public virtual string GetContextMenuHeader { get => Descricao; }

        public virtual object Clone()
        {
            Produto p = new Produto();

            p.Cod_Barra = Cod_Barra;
            p.Descricao = Descricao;
            p.Preco = Preco;
            p.Fornecedor = Fornecedor;
            p.Marca = Marca;
            p.Ncm = Ncm;
            p.Codigos = Codigos;

            return p;
        }
        public virtual string[] GetColunas()
        {
            return new string[] { "Cód. de Barras", "Descrição", "Preço", "Fornecedor", "Marca", "Ncm", "Cód. De Barras de Fornecedor" };
        }

        public virtual object GetId()
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
                        case "LastUpdate":
                            LastUpdate = DateTime.Parse(reader.ReadString());
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

                            Fornecedor = new FornecedorModel();

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

                            Marca = new MarcaModel();

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
            writer.WriteElementString("LastUpdate", LastUpdate.ToString("yyyy-MM-dd HH:mm:ss"));

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
