namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    class CadastrarBonusVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Criação de Bônus Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Bônus";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Bônus";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Bônus Foi Inserido Com Sucesso";
        }
    }
}
