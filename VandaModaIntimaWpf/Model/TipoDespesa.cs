using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class TipoDespesa : AModel, IModel
    {
        private Guid _id;
        private string _nome;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual Guid Id
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
