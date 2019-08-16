using NHibernate;
using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model.DAO
{
    public interface IDAO<T> where T : class, IModel
    {
        bool Inserir(T objeto);
        bool Inserir(IList<T> objetos);
        bool Atualizar(T objeto);
        bool InserirOuAtualizar(T objeto);
        bool Deletar(T objeto);
        bool Deletar(IList<T> objetos);
        IList<T> Listar();
        IList<T> Listar(ICriteria criteria);
        T ListarPorId(object id);
        ICriteria CriarCriteria();
    }
}
