namespace VandaModaIntimaWpf.Model
{
    public interface IModel
    {
        object GetIdentifier();
        string GetContextMenuHeader { get; }
    }
}
