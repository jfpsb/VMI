namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbTipoContagemLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public string Nome { get; set; }
        public CouchDbTipoContagemLog()
        {
            Tipo = "tipocontagem";
        }
    }
}
