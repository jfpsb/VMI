namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    class CadastrarAdiantamentoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Criação de Adiantamento Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Adiantamento";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Adiantamento";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Adiantamento Foi Inserido Com Sucesso";
        }
    }
}