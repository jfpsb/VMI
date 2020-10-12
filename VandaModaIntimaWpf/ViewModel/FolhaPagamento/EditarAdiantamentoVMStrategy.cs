using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class EditarAdiantamentoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Adiantamento";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Adiantamento Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Adiantamento";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Adiantamento Foi Atualizado Com Sucesso";
        }
    }
}
