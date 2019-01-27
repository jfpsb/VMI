using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntima.Model.DAO
{
    public interface IDAO<T> : IDisposable where T : class
    {
        bool Inserir(T objeto);
        bool Atualizar(T objeto);
        bool InserirOuAtualizar(T objeto);
        bool Deletar(T objeto);
        IList<T> Listar(ICriteria criteria);
        ICriteria CriarCriteria();
    }
}
