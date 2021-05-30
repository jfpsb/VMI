using NHibernate;
using System.Collections.ObjectModel;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Representante
{
    public class EditarRepresentanteVM : CadastrarRepresentanteVM
    {
        public EditarRepresentanteVM(ISession session, Model.Representante representante, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            viewModelStrategy = new EditarRepresentanteVMStrategy();
            Entidade = representante;
            Fornecedores = new ObservableCollection<Model.Fornecedor>(Entidade.Fornecedores);
        }
    }
}
