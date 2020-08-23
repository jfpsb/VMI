using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbMarcaLog : CouchDbLog
    {
        public string Nome { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public CouchDbMarcaLog()
        {
            Tipo = "marca";
        }
    }
}
