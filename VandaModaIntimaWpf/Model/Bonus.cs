using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Bonus : AModel, IModel
    {
        private long _id;
        private Funcionario _funcionario;
        private DateTime _data;
        private string _descricao;
        private double _valor;
        private int _mesReferencia;
        private int _anoReferencia;
        private double _base_calculo;
        private bool _bonusMensal;
        private bool _bonusCancelado;

        [JsonIgnore]
        public string GetContextMenuHeader => string.Format("R$ {0}", Valor);

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
        public Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }
        public string Descricao
        {
            get => _descricao?.ToUpper();
            set
            {
                _descricao = value;
                OnPropertyChanged("Descricao");
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

        public DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        [JsonIgnore]
        public string DataString
        {
            get => Data.ToString("G");
        }
        public int MesReferencia
        {
            get => _mesReferencia;
            set
            {
                _mesReferencia = value;
                OnPropertyChanged("MesReferencia");
            }
        }
        public int AnoReferencia
        {
            get => _anoReferencia;
            set
            {
                _anoReferencia = value;
                OnPropertyChanged("AnoReferencia");
            }
        }

        public double BaseCalculo
        {
            get => _base_calculo;
            set
            {
                _base_calculo = value;
                OnPropertyChanged("BaseCalculo");
            }
        }

        [JsonIgnore]
        public double HoraExtra100
        {
            get
            {
                if (Descricao.Equals("HORA EXTRA C/100%"))
                {
                    var valorHora = BaseCalculo / 220;
                    var qntHoras = Valor / (valorHora * 2);
                    return qntHoras;
                }

                return 0.0;
            }
        }

        [JsonIgnore]
        public double HoraExtra55
        {
            get
            {
                if (Descricao.Equals("HORA EXTRA C/055%"))
                {
                    var valorHora = BaseCalculo / 220;
                    var qntHoras = Valor / (valorHora * 1.55);
                    return qntHoras;
                }

                return 0.0;
            }
        }

        public bool BonusCancelado
        {
            get => _bonusCancelado;
            set
            {
                _bonusCancelado = value;
                OnPropertyChanged("BonusCancelado");
            }
        }

        public bool BonusMensal
        {
            get => _bonusMensal;
            set
            {
                _bonusMensal = value;
                OnPropertyChanged("BonusMensal");
            }
        }

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
        {
            throw new NotImplementedException("Bonus Não Possui Propriedades Que Usam Lazy Loading");
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
