using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class TipoContagem : AModel, ICloneable, IModel
    {
        private long _id;
        private string _nome;
        public bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(TipoContagem))
            {
                TipoContagem tipoContagem = (TipoContagem)obj;
                return tipoContagem.Id.Equals(Id)
                       && tipoContagem.Nome.Equals(Nome);
            }
            return false;
        }

        [JsonIgnore]
        public virtual string GetContextMenuHeader => Nome;

        [JsonIgnore]
        public Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Id", Id.ToString() }
                };

                return dic;
            }
        }

        [JsonProperty(PropertyName = "MySqlId")]
        public virtual long Id
        {
            get => _id;

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public string CouchDbId()
        {
            return Id.ToString();
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException("TipoContagem Não Possui Propriedades Com Lazy Loading");
        }
    }
}
