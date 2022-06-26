using NHibernate;
using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaVM : CadastrarLojaVM
    {
        public EditarLojaVM(ISession session, Model.Loja loja) : base(session, true)
        {
            viewModelStrategy = new EditarLojaVMStrategy();

            if (Entidade.Matriz == null)
                Entidade.Matriz = Matrizes[0];

            Entidade = loja;
            Aliquotas = new ObservableCollection<Model.AliquotasImposto>(Entidade.Aliquotas);
        }
    }
}
