using System;

namespace VandaModaIntimaWpf.Model
{
    public class TipoContagem : ObservableObject, ICloneable, IModel
    {
        private long _id;
        private string _nome;

        public bool IsIdentical(object obj)
        {
            if (obj != null && obj.GetType() == typeof(TipoContagem))
            {
                TipoContagem tipoContagem = (TipoContagem)obj;
                return tipoContagem.Id.Equals(Id)
                       && tipoContagem.Nome.Equals(Nome);
            }
            return false;
        }

        public virtual string GetContextMenuHeader => Nome;

        public virtual long Id
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

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual object GetIdentifier()
        {
            return Id;
        }
    }
}
