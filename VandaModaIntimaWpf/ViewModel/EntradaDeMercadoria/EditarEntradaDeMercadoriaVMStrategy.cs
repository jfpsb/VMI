namespace VandaModaIntimaWpf.ViewModel.EntradaDeMercadoria
{
    public class EditarEntradaDeMercadoriaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao atualizar entrada de mercadoria";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Entrada de mercadoria foi atualizada com sucesso.";
        }

        public string MessageBoxCaption()
        {
            return "Entrada de mercadoria";
        }
    }
}
