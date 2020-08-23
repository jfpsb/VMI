using Newtonsoft.Json;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public class CouchDbResponse
    {
        [JsonProperty(PropertyName = "ok")]
        public bool Ok { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "rev")]
        public string Rev { get; set; }
    }
}
