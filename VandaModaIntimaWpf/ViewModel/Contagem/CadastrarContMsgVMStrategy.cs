using System;

namespace VandaModaIntimaWpf.ViewModel.Contagem
{
    public class CadastrarContMsgVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoAtualizadoSucesso()
        {
            return "LOG de Atualização de Contagem Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoCriadoComSucesso()
        {
            return "LOG de Crição de Contagem Foi Criado Com Sucesso";
        }

        public string MensagemDocumentoNaoAtualizado()
        {
            return "Erro ao Criar LOG de Atualização de Contagem";
        }

        public string MensagemDocumentoNaoCriado()
        {
            return "Erro ao Criar LOG de Criação de Contagem";
        }

        public string MensagemEntidadeAtualizadaSucesso()
        {
            return "Contagem Foi Atualizada Com Sucesso";
        }

        public string MensagemEntidadeErroAoInserir()
        {
            return "Erro ao Inserir Contagem";
        }

        public string MensagemEntidadeInseridaSucesso()
        {
            return "Contagem Foi Inserida Com Sucesso";
        }

        public string MensagemEntidadeNaoAtualizada()
        {
            return "Erro ao Atualizar Contagem";
        }
    }
}
