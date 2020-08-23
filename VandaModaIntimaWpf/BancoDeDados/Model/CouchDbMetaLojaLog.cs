using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbMetaLojaLog : CouchDbLog
    {
        public string MySqlId { get; set; }
        public Loja Loja { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double Valor { get; set; }
        public CouchDbMetaLojaLog()
        {
            Tipo = "metaloja";
        }
    }
}
