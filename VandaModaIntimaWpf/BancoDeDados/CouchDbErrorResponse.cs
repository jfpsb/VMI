using Newtonsoft.Json;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public class CouchDbErrorResponse
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }
    }
}
