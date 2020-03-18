namespace VandaModaIntimaWpf.Model
{
    public interface IModel
    {
        object GetIdentifier();
        string GetDatabaseLogIdentifier();
        string GetContextMenuHeader { get; }
    }
}
