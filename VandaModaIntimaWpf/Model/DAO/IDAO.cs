using NHibernate;
using System;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO
{
    public interface IDAO<T>
    {
        bool Inserir(T objeto);
        bool Atualizar(T objeto);
        bool InserirOuAtualizar(T objeto);
        bool Deletar(T objeto);
        IList<T> Listar(ICriteria criteria);
        ICriteria CriarCriteria();
    }
}
