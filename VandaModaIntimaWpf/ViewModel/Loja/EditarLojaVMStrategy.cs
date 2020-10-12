using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Loja
{
    public class EditarLojaVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Loja";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Loja Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Loja";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Loja Foi Atualizada Com Sucesso";
        }
    }
}
