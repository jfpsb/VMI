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
        public virtual Produto Produto
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

        public virtual ICollection<SubGrade> SubGrades
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
        public virtual string SubGradesToString
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

        public virtual string CodBarra
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

        public virtual double Preco
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

        public virtual double PrecoCusto
        {
            get => _precoCusto;
            set
            {
                _precoCusto = value;
                OnPropertyChanged("PrecoCusto");
                OnPropertyChanged("MargemDeLucro");
            }
        }

        public virtual double MargemDeLucro
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

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => $"{CodBarra} - {SubGradesToString}";

        public virtual long Id
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

        public virtual object GetIdentifier()
        {
            return this;
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
