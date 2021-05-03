using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class SubGrade : AModel, IModel
    {
        private ProdutoGrade _produtoGrade;
        private Grade _grade;

        [JsonIgnore]
        public ProdutoGrade ProdutoGrade
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

        public Grade Grade
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

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => throw new NotImplementedException();

        public object GetIdentifier()
        {
            return this;
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException();
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(SubGrade))
            {
                SubGrade subGrade = obj as SubGrade;
                if (subGrade.ProdutoGrade.Id == ProdutoGrade.Id && subGrade.Grade.Id == Grade.Id)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            if (ProdutoGrade != null)
                hash += ProdutoGrade.GetHashCode();

            if (Grade != null)
                hash += Grade.GetHashCode();

            return hash;
        }
    }
}
