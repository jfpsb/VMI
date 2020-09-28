using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class ProdutoGrade : AModel, IModel
    {
        private string _codBarra;
        private Produto _produto;
        private double _preco;
        private IList<SubGrade> _subGrades = new List<SubGrade>();

        public Produto Produto
        {
            get
            {
                return _produto;
            }

            set
            {
                _produto = value;
                OnPropertyChanged("Produto");
            }
        }

        public IList<SubGrade> SubGrades
        {
            get
            {
                return _subGrades;
            }

            set
            {
                _subGrades = value;
                OnPropertyChanged("Grades");
            }
        }

        public string CodBarra
        {
            get
            {
                return _codBarra;
            }

            set
            {
                _codBarra = value;
                OnPropertyChanged("CodBarra");
            }
        }

        public double Preco
        {
            get
            {
                return _preco;
            }

            set
            {
                _preco = value;
                OnPropertyChanged("Preco");
            }
        }

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => $"{CodBarra}";

        public string CouchDbId()
        {
            return CodBarra;
        }

        public object GetIdentifier()
        {
            return this;
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
