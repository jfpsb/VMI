using SincronizacaoBD.Model;
using System;

namespace SincronizacaoBD.Sincronizacao
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

        public string GetFileName()
        {
            return $"{typeof(E).Name} {Entidade.GetIdentifier()}.json";
        }
    }
}
