using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.Sincronizacao
{
    public class DatabaseLogFile<E> where E : class, IModel
    {
        public string OperacaoMySQL { get; set; }
        public E Entidade { get; set; }
        public DateTime LastWriteTime { get; set; }

        public string GetClassName()
        {
            return typeof(E).Name;
        }
        public string GetIdentifier()
        {
            return Entidade.GetIdentifier().ToString();
        }
    }
}
