using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbParcelaLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public Adiantamento Adiantamento { get; set; }
        public FolhaPagamento Folhapagamento { get; set; }
        public int Numero { get; set; }
        public double Valor { get; set; }
        public bool Paga { get; set; }
        public CouchDbParcelaLog()
        {
            Tipo = "parcela";
        }
    }
}
