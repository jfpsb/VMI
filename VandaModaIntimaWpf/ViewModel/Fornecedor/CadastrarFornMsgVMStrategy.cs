namespace VandaModaIntimaWpf.ViewModel.Fornecedor
{
    class CadastrarFornMsgVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Fornecedor Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Criação de Fornecedor Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Fornecedor";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Fornecedor";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Fornecedor Foi Atualizado Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Fornecedor";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Fornecedor Foi Inserido Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Fornecedor";
        }
    }
}
