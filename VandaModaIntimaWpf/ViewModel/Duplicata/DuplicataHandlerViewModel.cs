using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VandaModaIntimaWpf.Model;
using DuplicataModel = VandaModaIntimaWpf.Model.Duplicata;

namespace VandaModaIntimaWpf.ViewModel.Duplicata
{
    public class DuplicataHandlerViewModel : ObservableObject
    {
        private ObservableCollection<DuplicataModel> _duplicatas;
        public DuplicataHandlerViewModel()
        {
            Duplicatas = new ObservableCollection<DuplicataModel>();

            DuplicataModel duplicata = new DuplicataModel()
            {
                TipoEntidade = typeof(Adiantamento),
                EntidadeLocal = new Adiantamento() { Id = 12379182371823 }
            };

            DuplicataModel duplicata3 = new DuplicataModel()
            {
                TipoEntidade = typeof(Bonus)
            };

            Duplicatas.Add(duplicata);
            Duplicatas.Add(duplicata3);
        }

        public ObservableCollection<DuplicataModel> Duplicatas
        {
            get
            {
                return _duplicatas;
            }

            set
            {
                _duplicatas = value;
                OnPropertyChanged("Duplicatas");
            }
        }
    }
}
