using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class AdicionarSalarioLiquidoVMStrategy : ICadastrarVMStrategy
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
            return "Erro Ao Inserir Salário Líquido";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Sucesso Ao Inserir Salário Líquido";
        }
    }
}
