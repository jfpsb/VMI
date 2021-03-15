﻿using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Adiantamento : AModel, IModel
    {
        private long _id;
        private Funcionario _funcionario;
        private DateTime _data;
        private double _valor;
        private string _descricao;
        private IList<Parcela> _parcelas = new List<Parcela>();

        [JsonIgnore]
        public string GetContextMenuHeader => _data.ToString("d") + " - " + _funcionario.Nome;

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
            get => _data.ToString("G");
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

        public IList<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

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

        public string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value?.ToUpper();
                OnPropertyChanged("Descricao");
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

            if (!NHibernateUtil.IsInitialized(Parcelas))
            {
                NHibernateUtil.Initialize(Parcelas);
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
