namespace SincronizacaoBD.Model
{
    public interface IModel
    {
        object GetId();
        string GetContextMenuHeader { get; }
    }
}
