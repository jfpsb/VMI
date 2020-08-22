using Newtonsoft.Json;
using NHibernate.Mapping;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbLog<E>
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("rev")]
        public string Rev { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("entidade")]
        public E Entidade { get; set; }

        [JsonProperty("revs_info")]
        public List<CouchDbRevStatus> RevsInfo = new List<CouchDbRevStatus>();
    }
}
