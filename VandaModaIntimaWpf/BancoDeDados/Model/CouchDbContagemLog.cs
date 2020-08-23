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
    }
}
