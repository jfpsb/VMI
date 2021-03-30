namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class CadastrarProdutoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Crição de Produto Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Produto";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Produto";
        }
        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Produto Foi Inserido Com Sucesso";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro de Produto";
        }
    }
}
