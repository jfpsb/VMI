using NHibernate;
using VandaModaIntimaWpf.ViewModel.Services.Interfaces;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class EditarDespesaVM : CadastrarDespesaVM
    {
        public EditarDespesaVM(ISession session, Model.Despesa despesa, IMessageBoxService messageBoxService) : base(session, messageBoxService, true)
        {
            Entidade = despesa;
            Entidade.TipoDespesa = despesa.TipoDespesa; //Necessário para executar o evento OnPropertyChanged dessa propriedade ao abrir a tela
            Entidade.Representante = despesa.Representante;
            Entidade.Fornecedor = despesa.Fornecedor;
            TipoDescricao = Entidade.Descricao;
            viewModelStrategy = new EditarDespesaVMStrategy();
        }
    }
}
