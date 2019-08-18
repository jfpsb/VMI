using NHibernate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.Model.DAO
{
    public interface IDAO<T> where T : class, IModel
    {
        Task<bool> Inserir(T objeto);
        Task<bool> Inserir(IList<T> objetos);
        Task<bool> Atualizar(T objeto);
        Task<bool> InserirOuAtualizar(T objeto);
        Task<bool> Deletar(T objeto);
        Task<bool> Deletar(IList<T> objetos);
        Task<IList<T>> Listar();
        Task<IList<T>> Listar(ICriteria criteria);
        Task<T> ListarPorId(object id);
        ICriteria CriarCriteria();
    }
}
