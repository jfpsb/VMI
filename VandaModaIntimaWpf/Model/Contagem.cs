using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Contagem : AModel, IModel
    {
        private long _id;
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

        [JsonIgnore]
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

        [JsonProperty(PropertyName = "MySqlId")]
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public string CouchDbId()
        {
            return Loja.CouchDbId() + Data.ToString();
        }

        public void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Contagens))
            {
                NHibernateUtil.Initialize(Contagens);
            }
        }
    }
}