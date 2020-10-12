using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class EditarFolhaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Folha de Pagamento";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Folha de Pagamento Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Folha de Pagamento";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Folha de Pagamento Foi Atualizada Com Sucesso";
        }
    }
}
