namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    class CadastrarFuncionarioVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Crição de Funcionário Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Funcionário";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Funcionário";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Funcionário Foi Inserido Com Sucesso";
        }
    }
}
