using System;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Loja
    {
        private string cnpj { get; set; }
        private Loja matriz { get; set; }
        private string nome { get; set; }
        private string telefone { get; set; }
        private string endereco { get; set; }
        private string inscricaoestadual { get; set; }
        private DateTime lastUpdate { get; set; } = DateTime.Now;
        public virtual string Cnpj
        {
            get { return cnpj; }
            set
            {
                cnpj = value;
            }
        }
        public virtual Loja Matriz
        {
            get { return matriz; }
            set
            {
                matriz = value;
            }
        }
        public virtual string Nome
        {
            get { return nome?.ToUpper(); }
            set
            {
                nome = value;
            }
        }
        public virtual string Telefone
        {
            get { return telefone; }
            set
            {
                telefone = value;
            }
        }
        public virtual string Endereco
        {
            get { return endereco?.ToUpper(); }
            set
            {
                endereco = value;
            }
        }
        public virtual string InscricaoEstadual
        {
            get { return inscricaoestadual; }
            set
            {
                inscricaoestadual = value;
            }
        }

        public virtual DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
            }
        }
    }
}
