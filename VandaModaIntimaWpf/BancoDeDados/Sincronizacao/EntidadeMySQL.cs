using System;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.BancoDeDados.Sincronizacao
{
    public class EntidadeMySQL<E> where E : class, IModel, ICloneable
    {
        public string OperacaoMySql { get; set; }
        public E EntidadeSalva { get; set; }
    }
}
