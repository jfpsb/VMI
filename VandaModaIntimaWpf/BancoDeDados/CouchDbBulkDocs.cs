using Newtonsoft.Json;
using System.Collections.Generic;
using VandaModaIntimaWpf.BancoDeDados.Model;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public class CouchDbBulkDocs
    {
        [JsonProperty(PropertyName = "all_or_nothing")]
        public bool AllOrNothing { get; set; } = true;
        [JsonProperty(PropertyName = "docs")]
        public IList<CouchDbLog> Docs { get; set; } = new List<CouchDbLog>();
    }
}
