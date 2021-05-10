using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarTotalVendidoVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            throw new NotImplementedException();
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            throw new NotImplementedException();
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Inserir Total Vendido";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Sucesso Ao Inserir Total Vendido";
        }

        public string MessageBoxCaption()
        {
            return "Adição de Valor Total Vendido";
        }
    }
}
