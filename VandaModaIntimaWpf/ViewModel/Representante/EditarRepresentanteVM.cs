using NHibernate;
using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class EditarRepresentanteVM : CadastrarRepresentanteVM
    {
        public EditarRepresentanteVM(ISession session, Model.Representante representante) : base(session, true)
        {
            viewModelStrategy = new EditarRepresentanteVMStrategy();
            Entidade = representante;
            Fornecedores = new ObservableCollection<Model.Fornecedor>(Entidade.Fornecedores);
        }
    }
}
