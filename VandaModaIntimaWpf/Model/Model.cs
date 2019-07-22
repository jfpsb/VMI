using System.Collections.Generic;

namespace VandaModaIntimaWpf.Model
{
    public interface Model<T> where T : class
    {
        bool Salvar();
        bool Atualizar();
        bool Deletar();
        IList<T> Listar();
        T ListarPorId(string id);
    }
}
