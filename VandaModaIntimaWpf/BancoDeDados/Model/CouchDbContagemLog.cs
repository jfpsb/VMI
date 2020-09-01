using System;
using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbContagemLog : CouchDbLog
    {
        public Loja Loja { get; set; }
        public DateTime Data { get; set; }
        public bool Finalizada { get; set; }
        public TipoContagem TipoContagem { get; set; }
        public IList<ContagemProduto> Contagens { get; set; } = new List<ContagemProduto>();
        public CouchDbContagemLog()
        {
            Tipo = "contagem";
        }
        public override void AtribuiCampos(object o)
        {
            Contagem c = (Contagem)o;

            Loja = c.Loja;
            Data = c.Data;
            Finalizada = c.Finalizada;
            TipoContagem = c.TipoContagem;
            Contagens = new List<ContagemProduto>(c.Contagens);
        }
        public override object Clone()
        {
            CouchDbContagemLog log = new CouchDbContagemLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                Loja = Loja,
                Data = Data,
                Finalizada = Finalizada,
                TipoContagem = TipoContagem,
                Contagens = new List<ContagemProduto>(Contagens)
            };

            return log;
        }
    }
}
