using NHibernate;
using System.Collections.ObjectModel;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaVM : CadastrarLojaVM
    {
        public EditarLojaVM(Model.Loja entidade, ISession session, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            viewModelStrategy = new EditarLojaVMStrategy();

            if (Entidade.Matriz == null)
                Entidade.Matriz = Matrizes[0];

            Entidade = entidade;
            Aliquotas = new ObservableCollection<Model.AliquotasImposto>(Entidade.Aliquotas);
        }
    }
}
