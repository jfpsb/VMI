namespace VandaModaIntimaWpf.Model
{
    public class Valor : AModel, IModel
    {
        private int _id;
        private double _original;

        public object GetIdentifier()
        {
            return Id;
        }

        public void InicializaLazyLoad()
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

        public string GetContextMenuHeader => throw new System.NotImplementedException();
    }
}
