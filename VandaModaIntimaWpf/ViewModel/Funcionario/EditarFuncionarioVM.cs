using NHibernate;
using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncionarioVM : CadastrarFuncionarioVM
    {
        public EditarFuncionarioVM(ISession session, Model.Funcionario funcionario) : base(session, true)
        {
            viewModelStrategy = new EditarFuncionarioVMStrategy();
            Entidade = funcionario;
            ChavesPix = new ObservableCollection<Model.ChavePix>(Entidade.ChavesPix);
            ContasBancarias = new ObservableCollection<Model.ContaBancaria>(Entidade.ContasBancarias);
        }
    }
}
