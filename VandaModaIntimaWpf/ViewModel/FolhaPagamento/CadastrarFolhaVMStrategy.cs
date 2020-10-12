﻿namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class CadastrarFolhaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Criação de Folha de Pagamento Foi Criado Com Sucesso";
        }
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Criação de Folha de Pagamento";
        }
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Inserir Folha de Pagamento";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Folha de Pagamento Foi Inserida Com Sucesso";
        }
    }
}
