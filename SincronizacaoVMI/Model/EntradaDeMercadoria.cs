using System;

namespace SincronizacaoVMI.Model
{
    public class EntradaDeMercadoria : AModel, IModel
    {
        private int _id;
        private Loja _loja;
        private DateTime _data;
        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
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
        public virtual DateTime Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
    }
}
