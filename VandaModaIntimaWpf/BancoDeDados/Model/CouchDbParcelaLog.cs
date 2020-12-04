using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbParcelaLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public Adiantamento Adiantamento { get; set; }
        public int Numero { get; set; }
        public double Valor { get; set; }
        public bool Paga { get; set; }
        public int MesAPagar { get; set; }
        public int AnoAPagar { get; set; }
        public CouchDbParcelaLog()
        {
            Tipo = "parcela";
        }
        public override void AtribuiCampos(object o)
        {
            Parcela p = (Parcela)o;
            MySqlId = p.Id;
            Adiantamento = p.Adiantamento;
            Numero = p.Numero;
            Valor = p.Valor;
            Paga = p.Paga;
            MesAPagar = p.MesAPagar;
            AnoAPagar = p.AnoAPagar;
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
                Numero = Numero,
                Valor = Valor,
                Paga = Paga,
                MesAPagar = MesAPagar,
                AnoAPagar = AnoAPagar
            };

            return log;
        }
    }
}
