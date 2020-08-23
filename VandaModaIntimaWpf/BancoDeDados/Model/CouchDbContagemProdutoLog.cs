using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Model
{
    public class CouchDbContagemProdutoLog : CouchDbLog
    {
        public long MySqlId { get; set; }
        public Contagem Contagem { get; set; }
        public Produto Produto { get; set; }
        public int Quant { get; set; }
        public CouchDbContagemProdutoLog()
        {
            Tipo = "contagemproduto";
        }
    }
}
