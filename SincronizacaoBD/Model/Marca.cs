using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SincronizacaoBD.Model
{
    public class Marca : IModel
    {
        private string nome { get; set; }
        private IList<Produto> produtos = new List<Produto>();

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value.ToUpper();
            }
        }

        [JsonIgnore]
        public virtual IList<Produto> Produtos
        {
            get { return produtos; }
            set
            {
                produtos = value;
            }
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return Nome;
        }
    }
}
