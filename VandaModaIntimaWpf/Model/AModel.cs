namespace VandaModaIntimaWpf.Model
{
    public abstract class AModel : ObservableObject
    {
        public virtual string Tipo => GetType().Name.ToLower();
    }
}
