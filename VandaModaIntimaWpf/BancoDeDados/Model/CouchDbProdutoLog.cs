using System.Collections.Generic;
using System.Linq;
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
        public override void AtribuiCampos(object o)
        {
            Produto p = (Produto)o;

            CodBarra = p.CodBarra;
            Fornecedor = p.Fornecedor;
            Marca = p.Marca;
            Descricao = p.Descricao;
            Preco = p.Preco;
            Ncm = p.Ncm;
            Codigos = p.Codigos.ToList();
        }
    }
}
