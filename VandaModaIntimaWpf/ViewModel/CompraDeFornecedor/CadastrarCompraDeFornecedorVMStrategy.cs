using System;

namespace VandaModaIntimaWpf.ViewModel.CompraDeFornecedor
{
    public class CadastrarCompraDeFornecedorVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Salvar Compra De Fornecedor!";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Compra De Fornecedor Foi Salva Com Sucesso!";
        }

        public string MessageBoxCaption()
        {
            return "Cadastro de Compra De Fornecedor";
        }
    }
}
