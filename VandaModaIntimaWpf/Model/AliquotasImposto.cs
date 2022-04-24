using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public class AliquotasImposto : AModel, IModel
    {
        private Guid _id;
        private DateTime _dataInsercao;
        private Loja _loja;
        private double _simples;
        private double _icms;

        public virtual Dictionary<string, string> DictionaryIdentifier => throw new NotImplementedException();

        public virtual string GetContextMenuHeader => throw new NotImplementedException();

        public virtual Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public virtual DateTime DataInsercao
        {
            get => _dataInsercao;
            set
            {
                _dataInsercao = value;
                OnPropertyChanged("DataInsercao");
            }
        }
        public virtual Loja Loja
        {
            get => _loja;
            set
            {
                _loja = value;
                OnPropertyChanged("Loja");
            }
        }
        public virtual double Simples
        {
            get => _simples;
            set
            {
                _simples = value;
                OnPropertyChanged("Simples");
            }
        }
        public virtual double Icms
        {
            get => _icms;
            set
            {
                _icms = value;
                OnPropertyChanged("Icms");
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
