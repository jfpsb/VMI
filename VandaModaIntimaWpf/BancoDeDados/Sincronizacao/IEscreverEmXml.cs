using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    public interface IEscreverEmXml<E> where E : class, IModel, ICloneable
    {
        void EscreverEmBinario(EntidadeMySQL<E> entidadeMySQL);
    }
}
