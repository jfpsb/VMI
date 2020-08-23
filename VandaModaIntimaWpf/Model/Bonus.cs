using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Bonus : ObservableObject, IModel
    {
        private long _id;
        private FolhaPagamento _folha;
        private DateTime _data;
        private string _descricao;
        private double _valor;

        [JsonIgnore]
        public string GetContextMenuHeader => string.Format("{0} - {1} - R$ {2}", Folha.MesReferencia, Folha.Funcionario.Nome, Valor);

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
        public FolhaPagamento Folha
        {
            get => _folha;
            set
            {
                _folha = value;
                OnPropertyChanged("Folha");
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

        public object GetIdentifier()
        {
            return Id;
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
