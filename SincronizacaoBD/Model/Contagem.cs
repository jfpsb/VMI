using Newtonsoft.Json;
using System;

namespace SincronizacaoBD.Model
{
    class Contagem : IModel
    {
        public virtual Loja Loja { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual bool Finalizada { get; set; }
        public virtual TipoContagem TipoContagem { get; set; }

        public virtual object GetIdentifier()
        {
            return Loja.Cnpj + Data.ToString("yyyyMMddHHmmss");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() == typeof(Contagem))
            {
                Contagem that = (Contagem)obj;

                if (Loja.Equals(that.Loja) && Data.Equals(that.Data))
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Loja.GetHashCode() + Data.GetHashCode();
        }
    }
}
