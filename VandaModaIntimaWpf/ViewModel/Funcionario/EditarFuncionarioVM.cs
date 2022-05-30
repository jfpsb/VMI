using System.Collections.ObjectModel;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncionarioVM : CadastrarFuncionarioVM
    {
        public EditarFuncionarioVM() : base()
        {
            //TODO: parametros, funcionario
            viewModelStrategy = new EditarFuncionarioVMStrategy();
            //Entidade = entidade;
            ChavesPix = new ObservableCollection<Model.ChavePix>(Entidade.ChavesPix);
            ContasBancarias = new ObservableCollection<Model.ContaBancaria>(Entidade.ContasBancarias);
        }
    }
}
