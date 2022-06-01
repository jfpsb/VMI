namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    class AdicionarBonusVMStrategy : ICadastrarVMStrategy
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

        public string MessageBoxCaption()
        {
            return "Cadastro de Bônus";
        }
    }
}
