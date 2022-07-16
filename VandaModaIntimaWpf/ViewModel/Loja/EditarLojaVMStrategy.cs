namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Loja";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Loja Foi Atualizada Com Sucesso";
        }

        public string MessageBoxCaption()
        {
            return "Edição de Loja";
        }
    }
}
