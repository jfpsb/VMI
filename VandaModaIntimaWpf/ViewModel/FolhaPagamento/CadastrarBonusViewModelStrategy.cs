namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    class CadastrarBonusViewModelStrategy : ICadastrarViewModelStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Bônus Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Criação de Bônus Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Bônus";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Bônus";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Bônus Foi Atualizado Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Bônus";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Bônus Foi Inserido Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Bônus";
        }
    }
}
