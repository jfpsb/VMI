using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class SubGrade : AModel, IModel
    {
        private long _id;
        private ProdutoGrade _produtoGrade;
        private Grade _grade;

        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [JsonIgnore]
        public virtual ProdutoGrade ProdutoGrade
        {
            get
            {
                return _produtoGrade;
            }

            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrade");
            }
        }

        public virtual Grade Grade
        {
            get
            {
                return _grade;
            }

            set
            {
                _grade = value;
                OnPropertyChanged("Grade");
            }
        }

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual object GetIdentifier()
        {
            return this;
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
