using Newtonsoft.Json;
using System;
using VandaModaIntimaWpf.Model.Converters;

namespace VandaModaIntimaWpf.Model
{
    public class TabelasINSS
    {
        [JsonConverter(typeof(TabelaINSSDataConverter))]
        public DateTime vigencia { get; set; }
        public double[] faixas { get; set; }
        public double[] porcentagens { get; set; }
    }
}
