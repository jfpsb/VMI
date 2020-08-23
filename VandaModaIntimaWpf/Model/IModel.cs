using Newtonsoft.Json;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public interface IModel
    {
        object GetIdentifier();

        [JsonIgnore]
        Dictionary<string, string> DictionaryIdentifier { get; }
        bool IsIdentical(object obj);

        [JsonIgnore]
        string GetContextMenuHeader { get; }
    }
}
