using NHibernate;
using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaVM : CadastrarLojaVM
    {
        public EditarLojaVM(ISession session) : base(session, false)
        {
            //TODO: parametro loja
            viewModelStrategy = new EditarLojaVMStrategy();

            if (Entidade.Matriz == null)
                Entidade.Matriz = Matrizes[0];

            //Entidade = entidade;
            Aliquotas = new ObservableCollection<Model.AliquotasImposto>(Entidade.Aliquotas);
        }
    }
}
