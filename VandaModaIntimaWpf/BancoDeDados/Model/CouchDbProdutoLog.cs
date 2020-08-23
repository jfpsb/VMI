using System.Collections.Generic;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbProdutoLog : CouchDbLog
    {
        public string CodBarra { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public Marca Marca { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public string Ncm { get; set; }
        public List<string> Codigos { get; set; } = new List<string>();

        public CouchDbProdutoLog()
        {
            Tipo = "produto";
        }
    }
}
