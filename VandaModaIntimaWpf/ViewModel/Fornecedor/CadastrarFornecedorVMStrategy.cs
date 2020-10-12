namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornecedorVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Criação de Fornecedor Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Fornecedor";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Fornecedor";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Fornecedor Foi Inserido Com Sucesso";
        }
    }
}
