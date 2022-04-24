using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class TipoGrade : AModel, IModel
    {
        private Guid _id;
        private string _nome;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => Nome;

        public virtual Guid Id
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

        public virtual string Nome
        {
            get
            {
                return _nome?.ToUpper();
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

        }

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
