using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class Despesa : AModel, IModel
    {
        private long _id;
        private TipoDespesa _tipoDespesa;
        private Fornecedor _fornecedor;
        private Representante _representante;
        private DateTime _data;
        private string _descricao;
        private double _valor;
        private string _familiar;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => $"{TipoDespesa.Nome} - {Valor}";

        public virtual long Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual TipoDespesa TipoDespesa
        {
            get => _tipoDespesa;
            set
            {
                _tipoDespesa = value;
                OnPropertyChanged("TipoDespesa");
            }
        }
        public virtual Fornecedor Fornecedor
        {
            get => _fornecedor;
            set
            {
                _fornecedor = value;
                OnPropertyChanged("Fornecedor");
            }
        }
        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public virtual string Descricao
        {
            get => _descricao;
            set
            {
                if (value != null)
                {
                    _descricao = value.ToUpper(); ;
                    OnPropertyChanged("Descricao");
                }
            }
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
        public virtual string Familiar
        {
            get => _familiar;
            set
            {
                _familiar = value;
                OnPropertyChanged("Familiar");
            }
        }

        public virtual Representante Representante
        {
            get => _representante;
            set
            {
                _representante = value;
                OnPropertyChanged("Representante");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
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
