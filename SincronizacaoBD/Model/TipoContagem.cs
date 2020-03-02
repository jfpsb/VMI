using Newtonsoft.Json;
using System;

namespace SincronizacaoBD.Model
{
    class TipoContagem : IModel
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }

        public virtual object GetIdentifier()
        {
            return Id.ToString();
        }
    }
}
