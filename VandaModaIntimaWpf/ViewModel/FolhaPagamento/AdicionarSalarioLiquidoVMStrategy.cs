namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarSalarioLiquidoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Salvar Documento Com Atualização de Salário Líquido";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "Sucesso ao Salvar Documento Com Atualização de Salário Líquido";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Inserir Salário Líquido";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Sucesso Ao Inserir Salário Líquido";
        }

        public string MessageBoxCaption()
        {
            return "Adição de Salário Líquido";
        }
    }
}
