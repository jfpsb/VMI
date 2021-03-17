using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarHoraExtraVMStrategy : ICadastrarVMStrategy
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
            return "Erro Ao Adicionar Hora Extra";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Sucesso Ao Adicionar Hora Extra";
        }
    }
}
