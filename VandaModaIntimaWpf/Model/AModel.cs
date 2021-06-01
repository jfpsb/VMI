namespace VandaModaIntimaWpf.Model
{
    public abstract class AModel : ObservableObject
    {
        private bool _deletado;
        public virtual string Tipo => GetType().Name.ToLower();

        public virtual bool Deletado
        {
            get => _deletado;
            set { _deletado = value;
                OnPropertyChanged("Deletado");
            }
        }
    }
}
