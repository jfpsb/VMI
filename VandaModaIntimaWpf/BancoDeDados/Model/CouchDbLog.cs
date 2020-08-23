using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbLog
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_rev", NullValueHandling = NullValueHandling.Ignore)]
        public string Rev { get; set; }

        [JsonProperty("_tipo")]
        public string Tipo { get; set; }

        [JsonProperty("_revs_info")]
        public List<CouchDbRevStatus> RevsInfo { internal get; set; } = new List<CouchDbRevStatus>();

        public virtual void AtribuiCampos(object o)
        {
            throw new NotImplementedException();
        }
    }
}
