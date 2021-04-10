using Newtonsoft.Json;
using System;

namespace VandaModaIntimaWpf
{
    public sealed class Config
    {
        private static readonly Lazy<Config> lazyClient = new Lazy<Config>(() => new Config());

        public static Config Instancia => lazyClient.Value;

        [JsonProperty("valor_diario_passagem_onibus")]
        public double ValorDiarioPassagemOnibus { get; set; }
        [JsonProperty("valor_diario_vale_alimentacao")]
        public double ValorDiarioValeAlimentacao { get; set; }
    }
}
