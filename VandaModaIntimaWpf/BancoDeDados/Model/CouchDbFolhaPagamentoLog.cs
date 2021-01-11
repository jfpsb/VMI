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
        public bool Fechada { get; set; }
        public CouchDbFolhaPagamentoLog()
        {
            Tipo = "folhapagamento";
        }
        public override void AtribuiCampos(object o)
        {
            FolhaPagamento fp = (FolhaPagamento)o;
            MySqlId = fp.Id;
            Mes = fp.Mes;
            Ano = fp.Ano;
            Funcionario = fp.Funcionario;
            Valor = fp.ValorATransferir;
            Fechada = fp.Fechada;
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
                Fechada = Fechada,
            };

            return log;
        }
    }
}
