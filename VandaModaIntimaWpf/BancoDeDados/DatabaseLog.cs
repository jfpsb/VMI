using System.Collections.Generic;

namespace VandaModaIntimaWpf.BancoDeDados
{
    public class DatabaseLog
    {
        public string Topico { get; set; }
        public Dictionary<string, string> Chaves { get; set; } = new Dictionary<string, string>();
        public bool EnviadoAoServidor { get; set; }
    }
}
