using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public interface IModel
    {
        object GetIdentifier();
        Dictionary<string, string> DictionaryIdentifier { get; }
        bool IsIdentical(object obj);
        string GetContextMenuHeader { get; }
    }
}
