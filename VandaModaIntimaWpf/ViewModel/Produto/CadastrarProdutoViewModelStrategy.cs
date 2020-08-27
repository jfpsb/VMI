namespace VandaModaIntimaWpf.ViewModel.Produto
{
    class CadastrarProdutoViewModelStrategy : ICadastrarViewModelStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Produto Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Crição de Produto Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Produto";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Produto";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Produto Foi Atualizado Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Produto";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Produto Foi Inserido Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Fornecedor";
        }
    }
}
