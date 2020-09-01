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
        public override void AtribuiCampos(object o)
        {
            Parcela p = (Parcela)o;
            MySqlId = p.Id;
            Adiantamento = p.Adiantamento;
            Folhapagamento = p.FolhaPagamento;
            Numero = p.Numero;
            Valor = p.Valor;
            Paga = p.Paga;
        }
        public override object Clone()
        {
            CouchDbParcelaLog log = new CouchDbParcelaLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                MySqlId = MySqlId,
                Adiantamento = Adiantamento,
                Folhapagamento = Folhapagamento,
                Numero = Numero,
                Valor = Valor,
                Paga = Paga
            };

            return log;
        }
    }
}
