namespace VandaModaIntimaWpf.Model
{
    public class Valor : AModel, IModel
    {
        private int _id;
        private double _original;

        public virtual object GetIdentifier()
        {
            return Id;
        }

        public virtual void InicializaLazyLoad()
        {
            throw new System.NotImplementedException();
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

        public virtual double Original
        {
            get
            {
                return _original;
            }

            set
            {
                _original = value;
                OnPropertyChanged("Original");
            }
        }

        public virtual string GetContextMenuHeader => throw new System.NotImplementedException();
    }
}
