using System;

namespace SincronizacaoVMI.Model
{
    public class TipoContagem : AModel, IModel
    {
        private int _id;
        private string _nome;
        public virtual int Id
        {
            get => _id;

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public virtual string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }
        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}
