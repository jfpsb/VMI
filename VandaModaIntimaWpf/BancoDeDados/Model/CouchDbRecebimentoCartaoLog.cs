using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbRecebimentoCartaoLog : CouchDbLog
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public Loja Loja { get; set; }
        public OperadoraCartao OperadoraCartao { get; set; }
        public double Recebido { get; set; }
        public double ValorOperadora { get; set; }
        public string Observacao { get; set; }
        public CouchDbRecebimentoCartaoLog()
        {
            Tipo = "recebimentocartao";
        }
    }
}
