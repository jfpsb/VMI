using Newtonsoft.Json;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public class CouchDbCredentials
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
