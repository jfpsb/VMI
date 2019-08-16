using VandaModaIntimaWpf.BancoDeDados.ConnectionFactory;
using VandaModaIntimaWpf.View;
using FornecedorModel = VandaModaIntimaWpf.Model.Fornecedor.Fornecedor;

namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class EditarFornecedorViewModel : CadastrarFornecedorViewModel, IEditarViewModel
    {
        private bool IsEditted = false;
        public override void Cadastrar(object parameter)
        {

            var result = IsEditted = Fornecedor.Atualizar();

            if (result)
            {
                ((IMessageable)parameter).MensagemDeAviso("Fornecedor Editado Com Sucesso");
                ((ICloseable)parameter).Close();
            }
            else
            {
                ((IMessageable)parameter).MensagemDeErro("Erro ao Editar Fornecedor");
            }
        }
        public bool EdicaoComSucesso()
        {
            return IsEditted;
        }

        public void PassaId(object Id)
        {
            Fornecedor = SessionProvider.GetSession().Load<FornecedorModel>(Id);
        }
    }
}
