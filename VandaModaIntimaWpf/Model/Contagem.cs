using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Contagem : ObservableObject, ICloneable, IModel
    {
        private Loja loja;
        private DateTime data;
        private bool finalizada;
        private TipoContagem tipoContagem;
        private IList<ContagemProduto> contagens = new List<ContagemProduto>();

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

        [JsonIgnore]
        public virtual IList<ContagemProduto> Contagens
        {
            get
            {
                return contagens;
            }

            set
            {
                contagens = value;
                OnPropertyChanged("Contagens");
            }
        }

        public void RefreshContagens()
        {
            OnPropertyChanged("Contagens");
        }

        public virtual object Clone()
        {
            Contagem contagem = new Contagem();

            contagem.Loja = Loja;
            contagem.Data = Data;
            contagem.TipoContagem = TipoContagem;
            contagem.Finalizada = Finalizada;

            return contagem;
        }

        public virtual object GetIdentifier()
        {
            return this;
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
            int hash = 0;

            if (Loja != null)
                hash += Loja.GetHashCode();

            if (Data != null)
                hash += Data.GetHashCode();

            return hash;
        }

        public string GetDatabaseLogIdentifier()
        {
            return Loja.Cnpj + Data.ToString("yyyyMMddHHmmss");
        }
    }
}
