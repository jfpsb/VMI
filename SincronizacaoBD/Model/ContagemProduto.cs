using Newtonsoft.Json;
using System;

namespace SincronizacaoBD.Model
{
    class ContagemProduto : IModel
    {
        public virtual long Id { get; set; }
        public virtual Contagem Contagem { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual int Quant { get; set; }

        public virtual object GetIdentifier()
        {
            return Id.ToString();
        }
    }
}
