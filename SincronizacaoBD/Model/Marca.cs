using System;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Marca : IModel
    {
        private string nome { get; set; }

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value.ToUpper();
            }
        }

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Nome;
        }
    }
}
