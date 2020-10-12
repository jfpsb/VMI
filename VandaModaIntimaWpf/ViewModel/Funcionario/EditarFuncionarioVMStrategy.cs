using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandaModaIntimaWpf.ViewModel.Funcionario
{
    public class EditarFuncionarioVMStrategy : ICadastrarVMStrategy
    {
        public string MensagemDocumentoNaoSalvo()
        {
            return "Erro ao Criar LOG de Atualização de Funcionário";
        }

        public string MensagemDocumentoSalvoComSucesso()
        {
            return "LOG de Atualização de Funcionário Foi Criado Com Sucesso";
        }

        public string MensagemEntidadeErroAoSalvar()
        {
            return "Erro ao Atualizar Fornecedor";
        }

        public string MensagemEntidadeSalvaComSucesso()
        {
            return "Funcionário Foi Atualizado Com Sucesso";
        }
    }
}
