using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbBonusLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public FolhaPagamento Folha { get; set; }
        public DateTime Data { get; set; }
        public string _Dscricao { get; set; }
        public double Valor { get; set; }
        public CouchDbBonusLog()
        {
            Tipo = "bonus";
        }
    }
}
