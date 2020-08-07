using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Contagem : ObservableObject, ICloneable, IModel
    {
        private Loja _loja;
        private DateTime _data;
        private bool _finalizada;
        private TipoContagem _tipoContagem;
        private IList<ContagemProduto> _contagens = new List<ContagemProduto>();

        public bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(Contagem))
            {
                Contagem contagem = (Contagem)obj;

                return contagem.Loja.Equals(Loja)
                       && contagem.Data.Equals(Data)
                       && contagem.Finalizada.Equals(Finalizada)
                       && contagem.TipoContagem.Equals(TipoContagem);
            }

            return false;
        }

        public Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Loja", Loja.Cnpj },
                    { "Data", Data.ToString("o") }
                };

                return dic;
            }
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader
        {
            get { return _loja.Cnpj; }
        }

        public virtual Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }

        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        public virtual bool Finalizada
        {
            get => _finalizada;
            set
            {
                _finalizada = value;
                OnPropertyChanged("Finalizada");
            }
        }

        public virtual TipoContagem TipoContagem
        {
            get => _tipoContagem;
            set
            {
                _tipoContagem = value;
                OnPropertyChanged("TipoContagem");
            }
        }

        public virtual IList<ContagemProduto> Contagens
        {
            get { return _contagens; }
            set
            {
                _contagens = value;
                OnPropertyChanged("Contagens");
            }
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
            if (obj == null) return false;
            if (obj.GetType() == typeof(Contagem))
            {
                Contagem that = (Contagem)obj;
                if (Loja.Equals(that.Loja) && Data.Equals(that.Data)) return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            if (Loja != null) hash += Loja.GetHashCode();
            hash += Data.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return Loja.ToString() + Data.ToString();
        }
    }
}