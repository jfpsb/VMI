using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class MembroFamiliar : AModel, IModel
    {
        private int _id;
        private Guid _uuid;
        private string _nome;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual int Id
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

        public virtual Guid Uuid
        {
            get
            {
                return _uuid;
            }

            set
            {
                _uuid = value;
                OnPropertyChanged("Uuid");
            }
        }

        public virtual string Nome
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
