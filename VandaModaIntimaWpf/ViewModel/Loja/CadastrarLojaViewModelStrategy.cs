namespace VandaModaIntimaWpf.ViewModel.Loja
{
    class CadastrarLojaViewModelStrategy : ICadastrarViewModelStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Loja Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Crição de Loja Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Loja";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Loja";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Loja Foi Atualizada Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Loja";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Loja Foi Inserida Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Fornecedor";
        }
    }
}
