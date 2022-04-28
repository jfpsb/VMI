using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Adiantamento : AModel, IModel
    {
        private int _id;
        private Funcionario _funcionario;
        private DateTime _data;
        private double _valor;
        private string _descricao;
        private IList<Parcela> _parcelas = new List<Parcela>();

        [JsonIgnore]
        public virtual string GetContextMenuHeader => _data.ToString("d") + " - " + _funcionario.Nome;

        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        [JsonIgnore]
        public virtual string DataString
        {
            get => _data.ToString("G");
        }
        public virtual double Valor
        {
            get => _valor;
            set
            {
                _valor = value;
                OnPropertyChanged("Valor");
            }
        }

        public virtual Funcionario Funcionario
        {
            get => _funcionario;
            set
            {
                _funcionario = value;
                OnPropertyChanged("Funcionario");
            }
        }

        public virtual IList<Parcela> Parcelas
        {
            get => _parcelas;
            set
            {
                _parcelas = value;
                OnPropertyChanged("Parcelas");
            }
        }

        [JsonIgnore]
        public virtual Dictionary<string, string> DictionaryIdentifier
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

        public virtual string Descricao
        {
            get => _descricao;
            set
            {
                _descricao = value?.ToUpper();
                OnPropertyChanged("Descricao");
            }
        }

        public virtual int Id
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
            return Id;
        }

        public virtual void InicializaLazyLoad()
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

        public virtual bool IsIdentical(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
