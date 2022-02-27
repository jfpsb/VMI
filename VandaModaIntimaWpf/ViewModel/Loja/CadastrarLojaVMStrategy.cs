namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class CadastrarLojaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Loja";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Loja Foi Inserida Com Sucesso";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro de Loja";
        }
    }
}
