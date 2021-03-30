using System;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarHoraExtraVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Salvar Documento De Hora Extra";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "Sucesso ao Salvar Documento De Hora Extra";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro Ao Adicionar Hora Extra";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Sucesso Ao Adicionar Hora Extra";
        }

        public string MessageBoxCaption()
        {
            return "Inserindo Hora Extra";
        }
    }
}
