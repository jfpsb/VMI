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
        public override void AtribuiCampos(object o)
        {
            RecebimentoCartao r = (RecebimentoCartao)o;
            Mes = r.Mes;
            Ano = r.Ano;
            Loja = r.Loja;
            OperadoraCartao = r.OperadoraCartao;
            Recebido = r.Recebido;
            ValorOperadora = r.ValorOperadora;
            Observacao = r.Observacao;
        }
        public override object Clone()
        {
            CouchDbRecebimentoCartaoLog log = new CouchDbRecebimentoCartaoLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                Mes = Mes,
                Ano = Ano,
                Loja = Loja,
                OperadoraCartao = OperadoraCartao,
                Recebido = Recebido,
                ValorOperadora = ValorOperadora,
                Observacao = Observacao
            };

            return log;
        }
    }
}
