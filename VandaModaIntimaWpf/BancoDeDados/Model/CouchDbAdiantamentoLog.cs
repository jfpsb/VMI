using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbAdiantamentoLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public Funcionario Funcionario { get; set; }
        public List<Parcela> Parcelas { get; set; } = new List<Parcela>();
        public CouchDbAdiantamentoLog()
        {
            Tipo = "adiantamento";
        }

        public override void AtribuiCampos(object o)
        {
            Adiantamento a = (Adiantamento)o;

            MySqlId = a.Id;
            Data = a.Data;
            Valor = a.Valor;
            Funcionario = a.Funcionario;
            Parcelas = new List<Parcela>(a.Parcelas);
        }
    }
}
