using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaVM : CadastrarLojaVM
    {
        public EditarLojaVM() : base()
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
