using NHibernate;

namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class EditarDespesaVM : CadastrarDespesaVM
    {
        public EditarDespesaVM(ISession session) : base(session, false)
        {
            //TODO:parametro, despesa
            //Entidade = despesa;
            //Entidade.TipoDespesa = despesa.TipoDespesa; //Necessário para executar o evento OnPropertyChanged dessa propriedade ao abrir a tela
            //Entidade.Representante = despesa.Representante;
            //Entidade.Fornecedor = despesa.Fornecedor;
            //if (despesa.DataVencimento != null)
            //{
            //    ultimaDataVencimento = despesa.DataVencimento; // Tem que ficar antes da próxima linha
            //    InserirVencimentoFlag = true;
            //}
            TipoDescricao = Entidade.Descricao;
            viewModelStrategy = new EditarDespesaVMStrategy();
        }
    }
}
