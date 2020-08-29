namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class CadastrarFolhaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Folha de Pagamento Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Criação de Folha de Pagamento Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Folha de Pagamento";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Folha de Pagamento";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Folha de Pagamento Foi Atualizada Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Folha de Pagamento";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Folha de Pagamento Foi Inserida Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Folha de Pagamento";
        }
    }
}
