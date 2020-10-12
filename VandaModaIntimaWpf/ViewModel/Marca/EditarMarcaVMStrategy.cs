using System;

namespace VandaModaIntimaWpf.ViewModel.Marca
{
    public class EditarMarcaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Marca";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Marca Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Marca";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Marca Foi Atualizada Com Sucesso";
        }
    }
}
