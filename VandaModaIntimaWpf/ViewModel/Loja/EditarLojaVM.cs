using NHibernate;
using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaVM : CadastrarLojaVM
    {
        public EditarLojaVM(Model.Loja entidade, ISession session) : base(session, true)
        {
            viewModelStrategy = new EditarLojaVMStrategy();

            if (Entidade.Matriz == null)
                Entidade.Matriz = Matrizes[0];

            Entidade = entidade;
            Aliquotas = new ObservableCollection<Model.AliquotasImposto>(Entidade.Aliquotas);
        }
    }
}
