using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class ProdutoGrade : AModel, IModel
    {
        private long _id;
        private string _codBarra;
        private Produto _produto;
        private double _preco;
        private double _precoCusto;
        private ICollection<SubGrade> _subGrades = new List<SubGrade>();

        [JsonIgnore]
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

        public ICollection<SubGrade> SubGrades
        {
            get
            {
                return _subGrades;
            }

            set
            {
                _subGrades = value;
                OnPropertyChanged("SubGrades");
            }
        }

        [JsonIgnore]
        public string SubGradesToString
        {
            get
            {
                string str = "";

                var enumerator = SubGrades.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    str += $"{enumerator.Current.Grade.TipoGrade.Nome} {enumerator.Current.Grade.Nome}";
                    str += "/";
                }

                if (!string.IsNullOrEmpty(str))
                    str = str.Remove(str.LastIndexOf("/"), 1);

                return str;
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

        public double PrecoCusto
        {
            get => _precoCusto;
            set
            {
                _precoCusto = value;
                OnPropertyChanged("PrecoCusto");
                OnPropertyChanged("MargemDeLucro");
            }
        }

        public double MargemDeLucro
        {
            get
            {
                if (PrecoCusto == 0.0)
                    return 0;

                return ((Preco - PrecoCusto) / Preco);
                //return Math.Truncate((Preco - PrecoCusto) / Preco * 10000) / 100;
            }
            set
            {
                if (PrecoCusto != 0.0)
                {
                    if (value >= 100)
                        MargemDeLucro = 99.9;

                    if (value < 0)
                        MargemDeLucro = 0;

                    Preco = (PrecoCusto / (1 - (value / 100))) / 100;
                }
            }
        }

        public Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public string GetContextMenuHeader => $"{CodBarra} - {SubGradesToString}";

        public long Id
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
