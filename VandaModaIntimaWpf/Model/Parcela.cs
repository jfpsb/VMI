using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Parcela : AModel, IModel
    {
        private long _id;
        private Adiantamento _adiantamento;
        private int _numero;
        private double _valor;
        private bool _paga;
        private int _mes;
        private int _ano;

        [JsonIgnore]
        public string GetContextMenuHeader => throw new NotImplementedException();

        [JsonIgnore]
        public Dictionary<string, string> DictionaryIdentifier
        {
            get
            {
                var dic = new Dictionary<string, string>
                {
                    { "Id", Id.ToString() }
                };

                return dic;
            }
        }

        public Adiantamento Adiantamento
        {
            get => _adiantamento;
            set
            {
                _adiantamento = value;
                OnPropertyChanged("Adiantamento");
            }
        }

        public double Valor
        {
            get => Math.Round(_valor, 2);
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }
        public bool Paga
        {
            get => _paga;
            set
            {
                _paga = value;
                OnPropertyChanged("Paga");
            }
        }

        public int Numero
        {
            get => _numero;
            set
            {
                _numero = value;
                OnPropertyChanged("Numero");
            }
        }

        public string NumeroComTotal
        {
            get => $"{Numero}/{Adiantamento.Parcelas.Count}";
        }

        [JsonProperty(PropertyName = "MySqlId")]
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
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

        [JsonIgnore]
        public string Vencimento
        {
            get
            {
                DateTime refer = new DateTime(Ano, Mes, 1);
                return refer.AddMonths(1).ToString("MM/yyyy");
            }
        }

        public string FolhaReferencia
        {
            get => $"{Mes}/{Ano}";
        }

        public object GetIdentifier()
        {
            return _id;
        }

        public void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Adiantamento))
            {
                NHibernateUtil.Initialize(Adiantamento);
            }
        }

        public bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }

        public string CouchDbId()
        {
            return Id.ToString();
        }
    }
}
