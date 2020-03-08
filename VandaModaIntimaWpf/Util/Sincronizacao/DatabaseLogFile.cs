using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.Util.Sincronizacao
{
    public class DatabaseLogFile<E> where E : class, IModel
    {
        public string OperacaoMySQL { get; set; }
        public E Entidade { get; set; }
        public DateTime LastWriteTime { get; set; }

        private string GetClassName()
        {
            return typeof(E).Name;
        }

        public string GetFileName()
        {
            return $"{GetClassName()} {Entidade.GetIdentifier()}.json";
        }
    }
}
