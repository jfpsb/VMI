using System.Linq;
using System.Xml.Serialization;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    public class EntidadeMySQL
    {
        public string OperacaoMySql { get; set; }
        [XmlIgnore]
        public IModel EntidadeSalva { get; set; }
        [XmlArray("interface")]
        [XmlArrayItem("ofTypeFornecedor", typeof(Fornecedor))]
        [XmlArrayItem("ofTypeLoja", typeof(Loja))]
        [XmlArrayItem("ofTypeMarca", typeof(Marca))]
        [XmlArrayItem("ofTypeOperadoraCartao", typeof(OperadoraCartao))]
        [XmlArrayItem("ofTypeProduto", typeof(Produto))]
        [XmlArrayItem("ofTypeRecebimentoCartao", typeof(RecebimentoCartao))]
        public object[] EntidadeSalvaSerialization
        {
            get { return new[] { EntidadeSalva }; ; }
            set { EntidadeSalva = (IModel)value.Single(); }
        }
    }
}
