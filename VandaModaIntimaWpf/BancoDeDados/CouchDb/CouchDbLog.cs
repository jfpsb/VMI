using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.BancoDeDados.CouchDb
{
    public class CouchDbLog
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_rev", NullValueHandling = NullValueHandling.Ignore)]
        public string Rev { get; set; }

        [JsonProperty(PropertyName = "_deleted")]
        public bool Deleted { get; set; }

        public string TipoEntidade { get; set; }

        [JsonProperty("_revs_info")]
        public List<CouchDbRevStatus> RevsInfo { internal get; set; } = new List<CouchDbRevStatus>();

        public bool Sincronizado { get; set; }
        public DateTime UltimaAlteracao { get; set; }
        public string Operacao { get; set; }
    }
}
