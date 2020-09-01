using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.Services.Interfaces
{
    public interface IAPesquisarVMCRUDService<E> where E : class, IModel
    {
        Task<bool> Deletar(object entidade);
        Task<bool> Deletar(IList<E> entidades);
        ObservableCollection<EntidadeComCampo<E>> PesquisaItens(int pesquisaIndex, string termo);
    }
}
