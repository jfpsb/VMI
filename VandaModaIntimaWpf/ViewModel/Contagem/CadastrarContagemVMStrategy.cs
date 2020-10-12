namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    public class CadastrarContagemVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Crição de Contagem Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Contagem";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Contagem";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Contagem Foi Inserida Com Sucesso";
        }
    }
}
