using Newtonsoft.Json;
using System;
using VandaModaIntimaWpf.Model.Converters;

namespace VandaModaIntimaWpf.Model
{
    public class DataFeriado
    {
        [JsonConverter(typeof(CalculoPassagemDataConverter)), JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type_code")]
        public string TypeCode { get; set; }
    }
}
