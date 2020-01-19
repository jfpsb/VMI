namespace VandaModaIntimaWpf.Model
{
    public interface IModel
    {
        object GetId();
        string GetContextMenuHeader { get; }
    }
}
