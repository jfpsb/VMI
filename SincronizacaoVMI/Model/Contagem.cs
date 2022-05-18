using System;

namespace SincronizacaoVMI.Model
{
    public class Contagem : AModel, IModel
    {
        private int _id;
        private Loja _loja;
        private DateTime _data;
        private bool _finalizada;
        private TipoContagem _tipoContagem;

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

        public virtual bool Finalizada
        {
            get => _finalizada;
            set
            {
                _finalizada = value;
                OnPropertyChanged("Finalizada");
            }
        }

        public virtual TipoContagem TipoContagem
        {
            get => _tipoContagem;
            set
            {
                _tipoContagem = value;
                OnPropertyChanged("TipoContagem");
            }
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

        public virtual void Copiar(object source)
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}