namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    class CadastrarAdiantamentoViewModelStrategy : ICadastrarViewModelStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Adiantamento Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Criação de Adiantamento Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Adiantamento";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Adiantamento";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Adiantamento Foi Atualizado Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Adiantamento";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Adiantamento Foi Inserido Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Adiantamento";
        }
    }
}