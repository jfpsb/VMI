using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbFolhaPagamentoLog : CouchDbLog
    {
        public string MySqlId { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public Funcionario Funcionario { get; set; }
        public double Valor { get; set; }
        public double ValorAPagar { get; set; }
        public bool Fechada { get; set; }
        public IList<Parcela> Parcelas { get; set; } = new List<Parcela>();
        public IList<Bonus> Bonus { get; set; } = new List<Bonus>();
        public CouchDbFolhaPagamentoLog()
        {
            Tipo = "folhapagamento";
        }
        public override object Clone()
        {
            CouchDbFolhaPagamentoLog log = new CouchDbFolhaPagamentoLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                MySqlId = MySqlId,
                Mes = Mes,
                Ano = Ano,
                Funcionario = Funcionario,
                Valor = Valor,
                ValorAPagar = ValorAPagar,
                Fechada = Fechada,
                Parcelas = new List<Parcela>(Parcelas),
                Bonus = new List<Bonus>(Bonus)
            };

            return log;
        }
    }
}
