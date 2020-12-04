using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbBonusLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public Funcionario Funcionario { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public CouchDbBonusLog()
        {
            Tipo = "bonus";
        }
        public override void AtribuiCampos(object o)
        {
            Bonus b = (Bonus)o;

            MySqlId = b.Id;
            Funcionario = b.Funcionario;
            Data = b.Data;
            Descricao = b.Descricao;
            Valor = b.Valor;
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
                Funcionario = Funcionario,
                Data = Data,
                Descricao = Descricao,
                Valor = Valor
            };

            return log;
        }
    }
}
