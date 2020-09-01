using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbLog : ICloneable
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_rev", NullValueHandling = NullValueHandling.Ignore)]
        public string Rev { get; set; }

        [JsonProperty(PropertyName = "_deleted")]
        public bool Deleted { get; set; }

        public string Tipo { get; set; }

        [JsonProperty("_revs_info")]
        public List<CouchDbRevStatus> RevsInfo { internal get; set; } = new List<CouchDbRevStatus>();

        public virtual void AtribuiCampos(object o)
        {
            throw new NotImplementedException("Atribui Campos Não Foi Implementado: " + GetType().Name);
        }

        public virtual object Clone()
        {
            throw new NotImplementedException("Clone Não Foi Implementado: " + GetType().Name);
        }
    }
}
