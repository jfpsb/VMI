namespace VandaModaIntimaWpf.ViewModel.Despesa
{
    public class CadastrarDespesaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Cadastrar Despesa!";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Despesa Foi Cadastrada Com Sucesso!";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro De Despesa";
        }
    }
}
