using System.Xml.Serialization;

namespace SincronizacaoBD.Sincronizacao
{
    [XmlRoot(ElementName = "EntidadeMySQL")]
    public class EntidadeMySQL<E> where E : class
    {
        public string OperacaoMySql { get; set; }
        public E EntidadeSalva { get; set; }
    }
}
