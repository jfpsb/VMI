using Newtonsoft.Json;
using System;
using VandaModaIntimaWpf.Model.Converters;

namespace VandaModaIntimaWpf.Model
{
    public class TabelasINSS
    {
        [JsonConverter(typeof(TabelaINSSDataConverter)), JsonProperty("vigencia")]
        public DateTime Vigencia { get; set; }

        [JsonProperty("faixas")]
        public double[] Faixas { get; set; }

        [JsonProperty("porcentagens")]
        public double[] Porcentagens { get; set; }

        [JsonProperty("salariofamilia")]
        public double SalarioFamilia { get; set; }
    }
}
