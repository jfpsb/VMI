namespace VandaModaIntimaWpf.Model
{
    public interface IModel
    {
        object GetIdentifier();
        bool IsIdentical(object obj);
        string GetContextMenuHeader { get; }
    }
}
