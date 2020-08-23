namespace VandaModaIntimaWpf.Model
{
    public abstract class AModel : ObservableObject
    {
        public string Tipo => GetType().Name.ToLower();
    }
}
