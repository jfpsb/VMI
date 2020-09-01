using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

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
        public override void AtribuiCampos(object o)
        {
            OperadoraCartao c = (OperadoraCartao)o;
            Nome = c.Nome;
            IdentificadoresBanco = new List<string>(c.IdentificadoresBanco);
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
