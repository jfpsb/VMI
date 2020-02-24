using Newtonsoft.Json;
using System;

namespace SincronizacaoBD.Model
{
    public class Marca : IModel
    {
        private string nome { get; set; }

        public virtual string Nome
        {
            get { return nome; }
            set
            {
                nome = value.ToUpper();
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
