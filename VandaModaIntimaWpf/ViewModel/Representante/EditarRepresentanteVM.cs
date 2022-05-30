using NHibernate;
using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class EditarRepresentanteVM : CadastrarRepresentanteVM
    {
        public EditarRepresentanteVM() : base()
        {
            //TODO: parametro representante
            viewModelStrategy = new EditarRepresentanteVMStrategy();
            //Entidade = representante;
            Fornecedores = new ObservableCollection<Model.Fornecedor>(Entidade.Fornecedores);
        }
    }
}
