namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    class CadastrarFuncMsgVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Funcionário Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Crição de Funcionário Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Funcionário";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Funcionário";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Funcionário Foi Atualizado Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Funcionário";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Funcionário Foi Inserido Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Fornecedor";
        }
    }
}
