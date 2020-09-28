using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class TipoGrade : AModel, IModel
    {
        private int _id;
        private string _nome;

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => Nome;

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Nome
        {
            get
            {
                return _nome;
            }

            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }

        public string CouchDbId()
        {
            return Id.ToString();
        }

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {
            
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
