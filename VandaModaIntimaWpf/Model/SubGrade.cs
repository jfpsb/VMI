using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class SubGrade : AModel, IModel
    {
        private ProdutoGrade _produtoGrade;
        private Grade _grade;

        public ProdutoGrade ProdutoGrade
        {
            get
            {
                return _produtoGrade;
            }

            set
            {
                _produtoGrade = value;
                OnPropertyChanged("ProdutoGrande");
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
                OnPropertyChanged("Grande");
            }
        }

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => throw new NotImplementedException();

        public string CouchDbId()
        {
            return ProdutoGrade.CodBarra + Grade.Id;
        }

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
                if (subGrade.ProdutoGrade.CodBarra == ProdutoGrade.CodBarra && subGrade.Grade.Id == Grade.Id)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ProdutoGrade.GetHashCode() + Grade.GetHashCode();
        }
    }
}
