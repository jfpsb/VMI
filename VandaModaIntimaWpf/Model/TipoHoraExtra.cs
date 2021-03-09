using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model
{
    public class TipoHoraExtra : AModel, IModel
    {
        private long _id;
        private string _descricao;

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => throw new NotImplementedException();

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
        public string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
            }
        }

        public string CouchDbId()
        {
            throw new NotImplementedException();
        }

        public object GetIdentifier()
        {
            throw new NotImplementedException();
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
