using System;

namespace SincronizacaoVMI.Model
{
    public class AliquotasImposto : AModel, IModel
    {
        private int _id;
        private DateTime _dataInsercao;
        private Loja _loja;
        private double _simples;
        private double _icms;

        public virtual int Id
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
    }
}
