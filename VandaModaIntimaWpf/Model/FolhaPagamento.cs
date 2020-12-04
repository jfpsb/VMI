using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VandaModaIntimaWpf.Model
{
    public class FolhaPagamento : AModel, IModel
    {
        private string _id;
        private int _mes;
        private int _ano;
        private Funcionario _funcionario;
        private double _valor;
        private bool _fechada;
        private IList<Parcela> _parcelas = new List<Parcela>();

        [JsonIgnore]
        public string GetContextMenuHeader => _mes + "/" + _ano + " - " + _funcionario.Nome;

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
        public string MesReferencia
        {
            get => _mes + "/" + _ano;
        }
        public Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
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
        public bool Fechada
        {
            get => _fechada;
            set
            {
                _fechada = value;
                OnPropertyChanged("Fechada");
            }
        }

        [JsonProperty(PropertyName = "MySqlId")]
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [JsonIgnore]
        public IList<Parcela> Parcelas
        {
            get
            {
                var listaDeParcelas = Funcionario.Adiantamentos.SelectMany(s => s.Parcelas);
                var parcelas = listaDeParcelas.Where(w => w.MesAPagar == Mes && w.AnoAPagar == Ano);
                return parcelas.ToList();
            }
        }

        [JsonIgnore]
        public IList<Bonus> Bonus
        {
            get
            {
                return Funcionario.Bonus.Where(w => w.MesReferencia == Mes && w.AnoReferencia == Ano).ToList();
            }
        }

        public object GetIdentifier()
        {
            return _id;
        }

        public void InicializaLazyLoad()
        {
            if (!NHibernateUtil.IsInitialized(Funcionario))
            {
                NHibernateUtil.Initialize(Funcionario);
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
