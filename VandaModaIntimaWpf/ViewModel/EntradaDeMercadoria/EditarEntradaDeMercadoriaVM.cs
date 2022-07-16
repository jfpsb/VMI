using NHibernate;

namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    public class EditarEntradaDeMercadoriaVM : CadastrarEntradaDeMercadoriaVM
    {
        public EditarEntradaDeMercadoriaVM(ISession session, Model.EntradaDeMercadoria entrada) : base(session, true)
        {
            viewModelStrategy = new EditarEntradaDeMercadoriaVMStrategy();
            Entidade = entrada;
            Entradas = new System.Collections.ObjectModel.ObservableCollection<Model.EntradaMercadoriaProdutoGrade>(Entidade.Entradas);
        }
    }
}
