using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class DespesaGroupByDescricaoViewModel : ObservableObject
    {
        private ObservableCollection<Model.Despesa> _despesas;

        public DespesaGroupByDescricaoViewModel()
        {
            Despesas = new ObservableCollection<Model.Despesa>(new List<Model.Despesa>());
        }

        public ObservableCollection<Model.Despesa> Despesas
        {
            get
            {
                return _despesas;
            }

            set
            {
                _despesas = value;
                OnPropertyChanged("Despesas");
            }
        }
    }
}
