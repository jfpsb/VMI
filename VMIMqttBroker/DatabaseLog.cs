using System.Collections.Generic;

namespace VMIMqttBroker
{
    public class DatabaseLog
    {
        public string Classe { get; set; }
        public Dictionary<string, string> Chaves { get; set; } = new Dictionary<string, string>();
        public string Operacao { get; set; }
        public bool EnviadoAoServidor { get; set; }
    }
}
