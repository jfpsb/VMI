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
        public override object Clone()
        {
            CouchDbContagemProdutoLog log = new CouchDbContagemProdutoLog()
            {
                Id = Id,
                Rev = Rev,
                Deleted = Deleted,
                Tipo = Tipo,
                RevsInfo = RevsInfo,
                MySqlId = MySqlId,
                Contagem = Contagem,
                Produto = Produto,
                Quant = Quant
            };

            return log;
        }
    }
}
