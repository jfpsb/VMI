namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class CadastrarLojaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Criação de Loja Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Loja";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Loja";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Loja Foi Inserida Com Sucesso";
        }
    }
}
