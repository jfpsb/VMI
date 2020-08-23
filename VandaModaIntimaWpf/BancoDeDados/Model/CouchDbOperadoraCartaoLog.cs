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
    }
}
