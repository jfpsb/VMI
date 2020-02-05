using System;
using System.Xml.Serialization;

namespace SincronizacaoBD.Model
{
    [XmlRoot(ElementName = "EntidadeSalva")]
    public class Marca
    {
        private string nome { get; set; }
        private DateTime lastUpdate { get; set; } = DateTime.Now;

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value.ToUpper();
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
