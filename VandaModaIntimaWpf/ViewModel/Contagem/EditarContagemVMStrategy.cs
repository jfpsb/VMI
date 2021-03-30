using System;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    public class EditarContagemVMStrategy : ICadastrarVMStrategy
    {
        public string MessageBoxCaption()
        {
            return "Edição de Contagem";
        }

        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Contagem";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Contagem Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Contagem";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Contagem Foi Atualizada Com Sucesso";
        }
    }
}
