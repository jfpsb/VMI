using System.Collections.Generic;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbOperadoraCartaoLog : CouchDbLog
    {
        public string Nome { get; set; }
        public IList<string> IdentificadoresBanco { get; set; } = new List<string>();
        public CouchDbOperadoraCartaoLog()
        {
            Tipo = "operadoracartao";
        }
        public override object Clone()
        {
            CouchDbOperadoraCartaoLog log = new CouchDbOperadoraCartaoLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                Nome = Nome,
                IdentificadoresBanco = new List<string>(IdentificadoresBanco)
            };

            return log;
        }
    }
}
