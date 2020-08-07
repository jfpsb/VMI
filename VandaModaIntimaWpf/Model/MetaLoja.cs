using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class MetaLoja : ObservableObject, IModel
    {
        private string _id;
        private Loja _loja;
        private int _mes;
        private int _ano;
        private double _valor;

        public string GetContextMenuHeader => throw new NotImplementedException();

        public Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Id", Id }
                };

                return dic;
            }
        }

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }
        public int Mes
        {
            get => _mes;
            set
            {
                _mes = value;
                OnPropertyChanged("Mes");
            }
        }
        public int Ano
        {
            get => _ano;
            set
            {
                _ano = value;
                OnPropertyChanged("Ano");
            }
        }
        public double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public string MesReferencia
        {
            get => string.Format("{0}/{1} - {2}", _mes, _ano, _valor);
        }

        public object GetIdentifier()
        {
            return _id;
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
