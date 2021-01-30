using Newtonsoft.Json;

namespace VandaModaIntimaWpf.BancoDeDados.CouchDb
{
    public class CouchDbLogFindResult
    {
        [JsonProperty("docs")]
        public CouchDbLog[] Docs { get; set; }
    }
}
