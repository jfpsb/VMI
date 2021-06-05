namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class EditarDespesaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Salvar Despesa!";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Despesa Foi Salva Com Sucesso!";
        }

        public string MessageBoxCaption()
        {
            return "Edição De Despesa";
        }
    }
}
