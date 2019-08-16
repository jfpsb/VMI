using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public interface IModel<T> where T : class
    {
        bool Salvar();
        bool Salvar(IList<T> lista);
        bool Atualizar();
        bool Deletar();
        bool Deletar(IList<T> objetos);
        IList<T> Listar();
        T ListarPorId(string id);
        string[] GetColunas();
    }
}
