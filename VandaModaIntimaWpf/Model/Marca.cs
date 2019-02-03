using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Marca
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }

        public virtual IList<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
