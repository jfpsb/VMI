using Newtonsoft.Json;
using System;

namespace VandaModaIntimaWpf.Model
{
    class Contagem : ObservableObject, ICloneable, IModel
    {
        private Loja loja;
        private DateTime data;
        private bool finalizada;
        private TipoContagem tipoContagem;

        [JsonIgnore]
        public virtual string GetContextMenuHeader { get { return loja.Cnpj; } }

        public virtual Loja Loja
        {
            get { return loja; }
            set
            {
                loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public virtual DateTime Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                OnPropertyChanged("Data");
            }
        }

        public virtual bool Finalizada
        {
            get
            {
                return finalizada;
            }

            set
            {
                finalizada = value;
                OnPropertyChanged("Finalizada");
            }
        }

        public virtual TipoContagem TipoContagem
        {
            get
            {
                return tipoContagem;
            }

            set
            {
                tipoContagem = value;
                OnPropertyChanged("TipoContagem");
            }
        }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

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
