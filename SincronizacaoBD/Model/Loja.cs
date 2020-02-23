using System;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Loja : IModel
    {
        private string cnpj { get; set; }
        private Loja matriz { get; set; }
        private string nome { get; set; }
        private string telefone { get; set; }
        private string endereco { get; set; }
        private string inscricaoestadual { get; set; }
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

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Cnpj;
        }
    }
}
