namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    class CadastrarRecebimentoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOGs de Crição de Recebimentos Foram Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOGs de Criação de Recebimentos";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Recebimentos";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Recebimentos Foram Inseridos Com Sucesso";
        }
    }
}
