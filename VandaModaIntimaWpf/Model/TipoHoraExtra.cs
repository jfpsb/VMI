using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class TipoHoraExtra : AModel, IModel
    {
        private Guid _id;
        private string _descricao;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        [JsonProperty(PropertyName = "MySqlId")]
        public virtual Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
