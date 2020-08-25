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
        public override object Clone()
        {
            CouchDbMarcaLog log = new CouchDbMarcaLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                Nome = Nome,
                Fornecedor = Fornecedor
            };

            return log;
        }
    }
}
