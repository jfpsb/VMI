using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.FolhaPagamento
{
    public class EditarBonusVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Bônus";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Bônus Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Bônus";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Bônus Foi Atualizado Com Sucesso";
        }
    }
}
