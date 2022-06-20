using Newtonsoft.Json;

namespace VandaModaIntimaWpf.Model
{
    public interface IModel
    {
        object GetIdentifier();

        [JsonIgnore]
        string GetContextMenuHeader { get; }
        void InicializaLazyLoad();
    }
}
