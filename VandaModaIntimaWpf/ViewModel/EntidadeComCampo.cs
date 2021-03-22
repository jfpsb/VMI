using System.Collections.Generic;
using System.Linq;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel
{
    /// <summary>
    /// Classe para representar coleção de itens de tela de pesquisa com campo de binding para marcar em DataGrid
    /// </summary>
    /// <typeparam name="E">Tipo da Entidade</typeparam>
    public class EntidadeComCampo<E> : ObservableObject where E : class, IModel
    {
        private E entidade;
        private bool isChecked = false;

        public static IList<EntidadeComCampo<E>> CriarListaEntidadeComCampo(IList<E> entidades)
        {
            return entidades.Select(s => new EntidadeComCampo<E>() { Entidade = s }).ToList();
        }

        public E Entidade
        {
            get { return entidade; }
            set
            {
                entidade = value;
                OnPropertyChanged("Entidade");
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
    }
}
