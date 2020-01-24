using System.Collections.Generic;
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

        public EntidadeComCampo(E entidade)
        {
            this.entidade = entidade;
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
        public static IList<EntidadeComCampo<E>> ConverterIList(IList<E> entidades)
        {
            IList<EntidadeComCampo<E>> lista = new List<EntidadeComCampo<E>>();

            if (entidades != null)
            {
                foreach (E e in entidades)
                {
                    EntidadeComCampo<E> em = new EntidadeComCampo<E>(e);
                    lista.Add(em);
                }
            }

            return lista;
        }

        public static IList<E> ConverterIList(IList<EntidadeComCampo<E>> entidades)
        {
            IList<E> lista = new List<E>();

            if (entidades != null)
            {
                foreach (EntidadeComCampo<E> e in entidades)
                {
                    E en = e.Entidade;
                    lista.Add(en);
                }
            }

            return lista;
        }
    }
}
