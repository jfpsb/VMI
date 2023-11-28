using System;

namespace SincronizacaoVMI.Model
{
    public class Funcao : AModel, IModel
    {
        private int _id;
        private string _nome;
        private string _cbo;
        public virtual string GetContextMenuHeader => $"{Nome} - {Cbo}";

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

        public virtual string Nome
        {
            get
            {
                return _nome;
            }

            set
            {
                _nome = value;
                OnPropertyChanged("Nome");
            }
        }

        public virtual string Cbo
        {
            get
            {
                return _cbo;
            }

            set
            {
                _cbo = value;
                OnPropertyChanged("Cbo");
            }
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}
