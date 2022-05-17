namespace SincronizacaoVMI.Model
{
    public interface IModel
    {
        object GetIdentifier();
        void Copiar(object source);
    }
}
