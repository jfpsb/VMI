using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbBonusLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public FolhaPagamento Folha { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public CouchDbBonusLog()
        {
            Tipo = "bonus";
        }
        public override object Clone()
        {
            CouchDbBonusLog log = new CouchDbBonusLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                MySqlId = MySqlId,
                Folha = Folha,
                Data = Data,
                Descricao = Descricao,
                Valor = Valor
            };

            return log;
        }
    }
}
