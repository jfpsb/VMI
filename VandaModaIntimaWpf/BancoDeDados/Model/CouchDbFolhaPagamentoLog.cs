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
    }
}
