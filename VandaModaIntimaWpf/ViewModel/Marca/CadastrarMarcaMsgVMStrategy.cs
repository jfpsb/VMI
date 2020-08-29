namespace VandaModaIntimaWpf.ViewModel.Marca
{
    class CadastrarMarcaMsgVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Marca Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Crição de Marca Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Marca";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Marca";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Marca Foi Atualizada Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Marca";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Marca Foi Inserida Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Fornecedor";
        }
    }
}
