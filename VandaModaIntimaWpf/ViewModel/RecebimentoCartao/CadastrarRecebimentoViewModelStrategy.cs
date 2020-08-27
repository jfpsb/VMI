namespace VandaModaIntimaWpf.ViewModel.RecebimentoCartao
{
    class CadastrarRecebimentoViewModelStrategy : ICadastrarViewModelStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOGs de Atualização de Recebimentos Foram Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOGs de Crição de Recebimentos Foram Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOGs de Atualização de Recebimentos";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOGs de Criação de Recebimentos";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Recebimentos Foram Atualizados Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Recebimentos";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Recebimentos Foram Inseridos Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Fornecedor";
        }
    }
}
