using System;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Produto";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Produto Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Produto";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Produto Foi Atualizado Com Sucesso";
        }
    }
}
