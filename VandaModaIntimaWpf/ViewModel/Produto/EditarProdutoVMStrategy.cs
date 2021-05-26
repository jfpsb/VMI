using System;

namespace VandaModaIntimaWpf.ViewModel.Produto
{
    public class EditarProdutoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Produto";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Produto Foi Atualizado Com Sucesso";
        }

        public string MessageBoxCaption()
        {
            return "Edição de Produto";
        }
    }
}
