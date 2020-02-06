using System;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Marca
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
    }
}
